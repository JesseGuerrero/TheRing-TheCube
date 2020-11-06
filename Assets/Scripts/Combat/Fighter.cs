using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Update()
        {            
            if (target == null) return;
            if (!target.IsAlive()) return;            
            timeSinceLastAttack += Time.deltaTime;
            if (!GetIsInRange())
            {
                //GetComponent<ActionScheduler>().StartAction(this);
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                //When you are in range, stop moving and look at the target
                GetComponent<Mover>().Cancel();                
                AttackBehavior();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {            
            return (combatTarget != null && combatTarget.GetComponent<Health>().IsAlive());
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                timeSinceLastAttack = 0f;
                TriggerAttack();
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            //trigger resets to false at the end of the animation automatically. This will trigger animation -> hit event.
            GetComponent<Animator>().SetTrigger("Attack");
        }

        //Keyword hit for animation about attacking. The trigger is activated in the middle of the animation, calling this function. The animation trigger is "Hit".
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();            
        }

        public void Cancel()
        {
            print("Fighter cancelled");
            target = null;
            StopAttack();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
    }
}