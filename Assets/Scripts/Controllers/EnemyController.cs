using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Disabled,
    Startup,
    Attacking,
    Stealing,
    Leaving,
    Exit,
    Dead
}
namespace BEER2023.Enemy
{
    [RequireComponent(typeof(HealthManager))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("States")]
        public float movementSpeed = 4f;
        public float angularSpeed = 240f;
        public float stopDistance = .5f;

        public float timeToSteal = 5f;
        private float timeStolen = 0f;

        public EnemyState initialState = EnemyState.Startup;

        [ReadOnly] public EnemyState currentState;

        // Components
        private NavMeshAgent nvAgent;
        private NavMeshPath currentPath;
        private HealthManager hpMan;

        private SMBox objective;
        private Transform exitPoint;

        private Animator animator;

        private float waitTime = 0f;

        [Header("Boxes")]
        public GameObject box;
        public Transform position;
        private GameObject boxita;
        public SMBox_Pickable smBox_drop;

        private void OnEnable()
        {
            // Probably not used if pooling is going to be used with enemies.
            // Use OnStart.
        }

        private void Awake()
        {
            // Gather components here.
            animator = GetComponentInChildren<Animator>();
            nvAgent = GetComponent<NavMeshAgent>();
            hpMan = GetComponent<HealthManager>();
            hpMan.OnEliminated += () => ChangeState(EnemyState.Dead);
        }

        private void Start()
        {
            nvAgent.speed = movementSpeed;
            nvAgent.angularSpeed = angularSpeed;
            nvAgent.stoppingDistance = stopDistance;
            //ChangeState(initialState);
        }

        private void Update()
        {
            if (enabled) OnTickStatus();
        }

        private void OnTickStatus()
        {
            switch (currentState)
            {
                case EnemyState.Startup:
                    // Do nothin on startup then
                    //nvAgent.isStopped = true;
                    // Init everything then go to Idle
                    // ChangeState(EnemyState.Idle);
                    if (Time.timeSinceLevelLoad > waitTime)
                    {
                        ChangeState(EnemyState.Attacking);
                        //ChangeState(EnemyState.Attacking);
                    }
                    break;
                case EnemyState.Attacking:
                    // Go towards the objecvtive through a pre-calculated path.
                    // When the objective is reached, then play the grab animation with correct orientation
                    // Afterwards, if carrying the object, go to recovering. (Maybe same entrance)
                    if (!objective.IsEnabled) SetAttackTarget(GameDirector.Instance.RequestObjective());
                    else if (!nvAgent.pathPending)
                    {
                        if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
                        {
                            if (objective.IsEnabled && hpMan.isAlive)
                            {
                                ChangeState(EnemyState.Stealing);
                            }
                        }
                    }
                    break;
                case EnemyState.Stealing:
                    if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
                    {
                        if (objective.IsEnabled && hpMan.isAlive)
                        {
                            if (timeStolen >= timeToSteal) ChangeState(EnemyState.Leaving);
                            timeStolen += Time.deltaTime;
                        }
                        else if(!objective.IsEnabled)
                        {
                            ChangeState(EnemyState.Attacking);
                        }
                        else
                        {
                            Debug.Log("AHAAAA");
                        }
                    }
                    break;
                case EnemyState.Leaving:
                    // Go towards an exit. If the character reaches there, then remove a life.
                    // Finally, destroy or hide (pooling) this object.
                    if (!nvAgent.pathPending)
                    {
                        if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
                        {
                            if (!nvAgent.hasPath || nvAgent.velocity.sqrMagnitude == 0f)
                            {
                                //...
                                // GOLASOOOOOO
                                if (!hpMan.isAlive) return;
                                GameDirector.Instance.BoxScored();
                                ChangeState(EnemyState.Exit);
                                //ChangeState(EnemyState.Dead);
                            }
                        }
                    }
                    break;
                case EnemyState.Disabled:
                    // Do nothing until enabled again
                    // Its mostly for oof'd enemies.
                    // Autocalled

                    break;
            }
        }

        public void SwapState(EnemyState newState) => ChangeState(newState);
        private void ChangeState(EnemyState newState)
        {
            // OnExit
            // Recheck even if same state.
            //if (newState == currentState) return;
            if (!currentState.Equals(null))
            {
                switch (currentState)
                {
                    case EnemyState.Disabled:
                        break;
                    case EnemyState.Startup:
                        animator.SetLayerWeight(2, 1f);
                        break;
                    case EnemyState.Attacking:
                        break;
                    case EnemyState.Stealing:
                        break;
                    case EnemyState.Leaving:
                        break;
                    case EnemyState.Dead:
                        break;
                }
            }

            currentState = newState;

            // OnEnter
            switch (currentState)
            {
                case EnemyState.Startup:
                    timeStolen = 0f;
                    hpMan.Spawn();
                    nvAgent.isStopped = true;
                    // Visible
                    animator.SetTrigger("Jump");
                    waitTime = Time.timeSinceLevelLoad + 1f;
                    animator.SetLayerWeight(2, 0f);
                    break;
                case EnemyState.Attacking:
                    nvAgent.isStopped = false;
                    if (!objective.IsEnabled) SetAttackTarget(GameDirector.Instance.RequestObjective());
                    // Autoselect a new target?
                    break;
                case EnemyState.Stealing:
                    animator.SetLayerWeight(1, 1f);
                    timeStolen = 0f;
                    animator.SetTrigger("GrabBox");
                    nvAgent.updateRotation = false;
                    var newdire = objective.transform.position - transform.position;
                    newdire.y = 0f;
                    transform.forward = newdire;
                    boxita = Instantiate(box, position);
                    //boxita.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                    boxita.transform.localPosition = Vector3.zero;
                    break;
                case EnemyState.Leaving:
                    // Select a new exit automatically!
                    // Play recover animation
                    // Spawn the box!
                    SetExitTarget(GameDirector.Instance.RequestExit());
                    nvAgent.updateRotation = true;
                    animator.SetTrigger("HasBox");
                    break;
                case EnemyState.Dead:
                    nvAgent.enabled = false;
                    // nvAgent.isStopped = true;
                    //nvAgent.path = null;
                    // TODO check if this works fine.
                    //gameObject.SetActive(false);
                    Ragdoll(Vector3.up + transform.forward * 20f, true, ForceMode.Impulse);
                    if (boxita != null)
                    {
                        Instantiate(smBox_drop, boxita.transform.position, boxita.transform.rotation);
                        boxita.SetActive(false);
                        Destroy(boxita);
                        // Drop box.
                        // Dropped box
                    }
                    //Destroy(this.gameObject, 12f);
                    // Points get deduced. But not here.
                    break;
                case EnemyState.Exit:
                    animator.SetTrigger("Exit");
                    nvAgent.enabled = false;
                    nvAgent.updateRotation = true;
                    // rotation is same as spawnpoint
                    //transform.rotation = Quaternion.Inverse(exitPoint.rotation);
                    animator.SetLayerWeight(1, 0f);
                    animator.SetLayerWeight(2, 0f);
                    break;
            }
        }

        public void DisableCollider()
        {
            // Called from animation
            GetComponent<Collider>().enabled = false;
        }


        // Path is given by spawner?
        public void SetAttackTarget(SMBox attackableTarget)
        {
            if (attackableTarget == null)
            {
                SetAttackTarget(GameDirector.Instance.RequestObjective());
                return;
            }

            objective = attackableTarget;

            CalculatePath(attackableTarget.transform);
        }
        
        public void SetExitTarget(Transform attackableTarget)
        {
            exitPoint = attackableTarget;

            CalculatePath(attackableTarget);
        }

        private void CalculatePath(Transform target)
        {
            // Create a path towars target?
            NavMeshPath newPath = new NavMeshPath();
            if ((NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, newPath)))
            {
                currentPath = newPath;
                nvAgent.SetPath(currentPath);
            }
        }

        public void Alert(SMBox alerter)
        {
            if (!hpMan.isAlive) return;
            if (currentState == EnemyState.Stealing || currentState == EnemyState.Leaving || currentState == EnemyState.Dead) return;
            if (objective == null || Vector3.Distance(alerter.transform.position, transform.position)
                < Vector3.Distance(objective.transform.position, transform.position))
            {
                SetAttackTarget(alerter);
            }
        }
        public void Ragdoll(Vector3 force, bool destroy = true, ForceMode mode = ForceMode.Impulse)
        {
            // Enable ragdoll and add force
            // Diosable cllisions
            // Instantiate 
            // AAAAAAAA
            var rb = this.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.drag = 0f;
            rb.AddForce(force, mode);
            Destroy(animator);
            if (destroy) Destroy(this.gameObject, 12);
        }
    }
}