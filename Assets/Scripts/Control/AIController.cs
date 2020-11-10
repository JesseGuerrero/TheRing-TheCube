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
        //1 is completely passive, 2 is only when attacked then forgotten later, 3 is always aggressive
        [Range(0, 2)]
        public int agressionLevel;
        public float forgetAggro = 60f;

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
        float timeSinceAssailant = Mathf.Infinity;
        
        //Implement walking speed during patrol for NPCs
        public float patrolspeed;


        //make this not show in editor
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

            

            if (IsAggro() && InAttackRangeOfPlayer() && fighter.CanAttack(player))
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

            UpdateAssaultMemory();

            UpdateTimers();
        }

        private void UpdateAssaultMemory()
        {
            /*
            If we were assulted within memory and we see the player, reset the time
            If we were assaulted but forgot, then truely forget.             
             */
            if (health.GetIsAssaulted() && timeSinceAssailant > forgetAggro && InAttackRangeOfPlayer()) {
                timeSinceAssailant = 0;
            } else if(health.GetIsAssaulted() && timeSinceAssailant > forgetAggro)
            {
                health.SetIsAssaulted(false);
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            patrolDwellTime += Time.deltaTime;
            timeSinceAssailant += Time.deltaTime;
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

        private bool IsAggro()
        {            
            /*
            If AI is completely passive dont aggro
            If AI is aggro when attacked and remembers assailant then attack
            If always aggro then attack
            Otherwise don't attack.
             */
            if (agressionLevel == 0) return false;
            if (agressionLevel == 1 && (timeSinceAssailant < forgetAggro)) return true;
            if (agressionLevel == 2) return true;
            if (health.GetIsAssaulted()) return true;
            return false;
        }

        private bool InAttackRangeOfPlayer()
        {            
            return (Vector3.Distance(player.transform.position, transform.position) < chaseDistance);
        }

        //Called by Unity, a standard function
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}