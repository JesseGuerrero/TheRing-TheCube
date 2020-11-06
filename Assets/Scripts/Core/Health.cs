using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isAlive = true;

        public bool IsAlive() { return isAlive; }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints <= 0 && isAlive)
            {
                isAlive = false;
                Die();                
            }
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("Death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
