using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Startup,
    Spawning,
    Idle,
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

        public EnemyState initialState = EnemyState.Startup;

        private EnemyState currentState;

        // Components
        private NavMeshAgent nvAgent;
        private NavMeshPath currentPath;
        private HealthManager hpMan;

        private void OnEnable()
        {
            // Probably not used if pooling is going to be used with enemies.
            // Use OnStart.
        }

        private void Awake()
        {
            // Gather components here.
            nvAgent = GetComponent<NavMeshAgent>();
            hpMan = GetComponent<HealthManager>();
        }

        private void Start()
        {
            nvAgent.speed = movementSpeed;
            nvAgent.angularSpeed = angularSpeed;
            nvAgent.stoppingDistance = stopDistance;
            ChangeState(initialState);
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
                    break;
                case EnemyState.Idle:
                    // Maybe do something idk
                    break;
                case EnemyState.Attacking:
                    // Go towards the objecvtive through a pre-calculated path.
                    // When the objective is reached, then play the grab animation with correct orientation
                    // Afterwards, if carrying the object, go to recovering. (Maybe same entrance)
                    break;
                case EnemyState.Recovering:
                    // Go towards an exit. If the character reaches there, then remove a life.
                    // Finally, destroy or hide (pooling) this object.
                    break;
                case EnemyState.Disabled:
                    // Do nothing until enabled again
                    // Autocalled
                    break;
                case EnemyState.Spawning:
                    // 
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
                    case EnemyState.Startup:
                        break;
                    case EnemyState.Idle:
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
                case EnemyState.Startup:
                    nvAgent.isStopped = true;
                    break;
                case EnemyState.Idle:
                    break;
                case EnemyState.Attacking:
                    break;
                case EnemyState.Recovering:
                    break;
                case EnemyState.Disabled:
                    nvAgent.isStopped = true;
                    break;
                case EnemyState.Spawning:
                    // Play an animator and respawn?
                    // Atleast enable the GO.
                    hpMan.Spawn();
                    break;
            }
        }

        public void SetAttackTarget(Transform attackableTarget)
        {
            // Create a path towars target?
            NavMeshPath newPath = new NavMeshPath();
            if ((NavMesh.CalculatePath(transform.position, attackableTarget.position, NavMesh.AllAreas, newPath)))
            {
                currentPath = newPath;
                // Apply to agent too.
                nvAgent.SetPath(currentPath);
            }
        }
    }
}