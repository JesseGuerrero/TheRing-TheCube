using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if(!GetComponent<Health>().IsAlive())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                return;
            }
            if (InteractWithCombat()) return;
            if(InteractWithMovement()) return;            
        }

        private bool InteractWithMovement()
        {                        
            //out means save the variable by reference outside the function
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;            
        }

        private bool InteractWithCombat()
        {
            /*
             * RaycastAll gets all gits along the rayline. If one of them turns out to contain a CombatTarget script, then we have a new object to target
             * This way now we can click through other objects and focus on the enemy.
             */
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
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
            return false;
        }
                            
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}


