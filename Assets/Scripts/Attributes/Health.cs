using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        private void Start()
        {
            print(gameObject.name + " Health: " + health);
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0) Die();
        }

        private void Die()
        {
            print(gameObject + " has been destroyed.");
            Destroy(gameObject);
        }
    }
}