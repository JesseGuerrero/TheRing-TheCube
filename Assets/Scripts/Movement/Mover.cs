using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        NavMeshAgent navMeshAgent;
        Health health;


        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            UpdateAnimator();
            navMeshAgent.enabled = health.IsAlive();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);            
            MoveTo(destination);
        }        

        public void MoveTo(Vector3 destination)
        {            
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;            
        }        

        public void Cancel()
        {
            print("Mover cancelled");            
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            //convert global direction to local direction, https://bit.ly/3kS483h
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            //+z becuase thats forward locally
            float speed = localVelocity.z/5.66f;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
