using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Idle,
    Startup,
    Spawning,
    Attacking,
    Recovering,
    Disabled
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

        public EnemyState initialState = EnemyState.Spawning;

        [ReadOnly] public EnemyState currentState;

        // Components
        private NavMeshAgent nvAgent;
        private NavMeshPath currentPath;
        private HealthManager hpMan;

        private SMBox objective;

        private Animator animator;

        private float waitTime = 0f;

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
            hpMan.OnEliminated += () => ChangeState(EnemyState.Disabled);
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
                case EnemyState.Idle:
                    // Start seartching for one? Or just stay still.
                    break;
                case EnemyState.Startup:
                    // Do nothin on startup then
                    //nvAgent.isStopped = true;
                    // Init everything then go to Idle
                    // ChangeState(EnemyState.Idle);
                    if (Time.timeSinceLevelLoad > waitTime)
                    {
                        ChangeState(EnemyState.Spawning);
                        //ChangeState(EnemyState.Attacking);
                    }
                    
                    break;
                case EnemyState.Spawning:
                    // Waiting for animation to end.
                    ChangeState(EnemyState.Attacking);
                    break;
                case EnemyState.Attacking:
                    // Go towards the objecvtive through a pre-calculated path.
                    // When the objective is reached, then play the grab animation with correct orientation
                    // Afterwards, if carrying the object, go to recovering. (Maybe same entrance)
                    if (!objective.IsEnabled) ChangeState(EnemyState.Idle);
                    else if (!nvAgent.pathPending)
                    {
                        if (nvAgent.remainingDistance <= nvAgent.stoppingDistance)
                        {
                            if (objective.IsEnabled && hpMan.isAlive)
                            {
                                timeStolen += Time.deltaTime;
                                Debug.Log("Stealing! for: " + timeStolen);
                                if (timeStolen > timeToSteal) ChangeState(EnemyState.Recovering);
                            }
                        }
                    }
                    break;
                case EnemyState.Recovering:
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
                                ChangeState(EnemyState.Disabled);
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
            if (newState == currentState) return;
            if (!currentState.Equals(null))
            {
                switch (currentState)
                {
                    case EnemyState.Idle:
                        nvAgent.isStopped = false;
                        break;
                    case EnemyState.Startup:
                        break;
                    case EnemyState.Attacking:
                        break;
                    case EnemyState.Recovering:
                        break;
                    case EnemyState.Disabled:
                        break;
                    case EnemyState.Spawning:
                        break;
                }
            }

            currentState = newState;

            // OnEnter
            switch (currentState)
            {
                case EnemyState.Idle:
                    nvAgent.isStopped = true;
                    break;
                case EnemyState.Startup:
                    timeStolen = 0f;
                    hpMan.Spawn();
                    nvAgent.isStopped = true;
                    // Visible
                    animator.SetTrigger("Jump");
                    waitTime = Time.timeSinceLevelLoad + 1f;
                    break;
                case EnemyState.Attacking:
                    nvAgent.isStopped = false;
                    // Autoselect a new target?
                    break;
                case EnemyState.Recovering:
                    // Select a new exit automatically!
                    // Play recover animation
                    // TODO
                    SetAttackTarget(GameDirector.Instance.RequestExit());
                    break;
                case EnemyState.Disabled:
                    nvAgent.isStopped = true;
                    //nvAgent.path = null;
                    // TODO check if this works fine.
                    gameObject.SetActive(false);
                    // Points get deduced. But not here.
                    break;
                case EnemyState.Spawning:
                    // Play an animator and respawn?
                    // Atleast enable the GO.
                    // TODO check if fine.
                    //GetComponentInChildren<MeshRenderer>().enabled = enabled;
                   
                    break;
            }
        }

        // Path is given by spawner?
        public void SetAttackTarget(Transform attackableTarget)
        {
            if (attackableTarget == null)
            {
                ChangeState(EnemyState.Idle);
                return;
            }
            objective = attackableTarget.GetComponent<SMBox>();
            // Create a path towars target?
            NavMeshPath newPath = new NavMeshPath();
            if ((NavMesh.CalculatePath(transform.position, attackableTarget.position, NavMesh.AllAreas, newPath)))
            {
                currentPath = newPath;
                // Apply to agent too.
                nvAgent.SetPath(currentPath);
            }
            //if (currentState == EnemyState.Idle) ChangeState(EnemyState.Attacking);
        }

        public void Alert(SMBox alerter)
        {
            if (objective == null || Vector3.Distance(alerter.transform.position, transform.position)
                < Vector3.Distance(objective.transform.position, transform.position))
            {
                SetAttackTarget(alerter.transform);
            }
        }
        public void Ragdoll(Vector3 force)
        {
            // Enable ragdoll and add force
            // Diosable cllisions
            // Instantiate 
            // AAAAAAAA
        }
    }
}