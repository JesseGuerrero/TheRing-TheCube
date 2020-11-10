using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isAlive = true;
        bool isAssaulted = false;

        public bool IsAlive() { return isAlive; }
        
        //remove from inspector
        public void SetIsAssaulted(bool setBool) { isAssaulted = setBool; }
        public bool GetIsAssaulted() { return isAssaulted; }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            UpdateAssaulted();

            if (healthPoints <= 0 && isAlive)
            {
                isAlive = false;
                Die();
            }
        }

        private void UpdateAssaulted()
        {
            if (!GetIsAssaulted() && healthPoints > 0) { GetComponent<IAudio>().StartAggroSound(); SetIsAssaulted(true); }
        }

        private void Die()
        {
            GetComponent<IAudio>().DeathSound();
            GetComponent<Animator>().SetTrigger("Death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
