using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {                                
        //Merge with FollowCam, optimize mouse look, make a prefab for 1st and 3rd person cams, update player prefab.
        PlayerCamController camController;

        private void Start()
        {                       
            camController = GetComponent<PlayerCamController>();
        }
        private void Update()
        {
            if(!GetComponent<Health>().IsAlive())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                return;
            }

            //We are either in combat or in movement.
            if (InteractWithCombat()) return;
            if(InteractWithMovement()) return;            
        }

        private bool InteractWithMovement()
        {
            if (camController.IsFirst())
            {//Now we are in first person

                /*
                 Remove crosshair, get movement change
                 */
                camController.crosshairUI.SetActive(true);                
                Vector3 destination = transform.position + getControllerMovement();
                GetComponent<Mover>().StartMoveAction(destination);
                return true;
            }
            else
            {//third person
                camController.crosshairUI.SetActive(false);

                GetComponent<NavMeshAgent>().angularSpeed = 5000f;
                //out means save the variable by reference outside the function
                RaycastHit hit;
                bool hasHit = Physics.Raycast(camController.GetMouseRay(), out hit);
                if (hasHit)
                {
                    if (Input.GetMouseButton(0))
                    {
                        GetComponent<Mover>().StartMoveAction(hit.point);
                    }
                    return true;
                }
            }

            return false;            
        }

        private bool InteractWithCombat()
        {
            if (camController.IsFirst())
            {//First person
                /*
                 * Raycast All but if one has a combat target the first one is the one we attack. We will see through object though.                   
                 */
                RaycastHit[] hits = Physics.RaycastAll(camController.GetCenterRay());
                Fighter fighterScript = GetComponent<Fighter>();
                foreach (RaycastHit item in hits)
                {
                    CombatTarget target = item.collider.GetComponent<CombatTarget>();
                    if (target == null) continue;

                    if (!fighterScript.CanAttack(target.gameObject)) continue;

                    if (Input.GetMouseButton(0))
                    {
                        fighterScript.Attack(target.gameObject);
                        return true;
                    }

                    //if attacking, continue attacking by returning that you are still in combat
                    if (GetComponent<Fighter>().getCombatTarget() != null) { return true; }
                }                
            }
            else
            {//Third person
                /*
                 * RaycastAll gets all gits along the rayline. If one of them turns out to contain a CombatTarget script, then we have a new object to target
                 * This way now we can click through other objects and focus on the enemy.
                 */
                RaycastHit[] hits = Physics.RaycastAll(camController.GetMouseRay());
                Fighter fighterScript = GetComponent<Fighter>();
                foreach (RaycastHit item in hits)
                {
                    CombatTarget target = item.collider.GetComponent<CombatTarget>();
                    if (target == null) continue;

                    if (!fighterScript.CanAttack(target.gameObject)) continue;

                    if (Input.GetMouseButton(0))
                    {
                        fighterScript.Attack(target.gameObject);
                    }
                    return true;
                }
            }

            return false;
        }
        private Vector3 getControllerMovement()
        {
            /*
             Remove turning
             Get (xDir, 0, 0) vectors
            (yDir, 0, 0) vectors where 1 or -1
             */

            GetComponent<NavMeshAgent>().angularSpeed = 0f;
            float xDir = Input.GetAxis("Horizontal");
            float zDir = Input.GetAxis("Vertical");
            Vector3 movement = transform.right * xDir + transform.forward * zDir;
            return movement;
        }
    }
}


