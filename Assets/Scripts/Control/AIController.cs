using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control { 
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float patrolDwellTimes = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float patrolDwellTime = Mathf.Infinity;
        public int waypointIndex = 0;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;            
        }

        private void Update()
        {
            if (!health.IsAlive())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                return;
            }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {                
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            patrolDwellTime += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if(AtWayPoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(patrolDwellTime >= patrolDwellTimes) mover.StartMoveAction(nextPosition);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());            
            return (distanceToWaypoint < waypointTolerance);
        }

        private void CycleWaypoint()
        {
            patrolDwellTime = 0;
            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {            
            return (Vector3.Distance(player.transform.position, transform.position) < 5);
        }

        //Called by Unity, a standard function
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}