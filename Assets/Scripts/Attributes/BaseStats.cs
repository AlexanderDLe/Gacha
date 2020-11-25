using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.Attributes
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 50)]
        public int level = 1;
        public Progression progression = null;
        public float maxHealth = 100f;
        public float currentHealth = 100f;
        public float damage = 5f;

        public void Initialize(BaseCharacter_SO charScript)
        {
            this.progression = charScript.progression;
            SetHealth();
            SetDamage();
        }

        private void SetHealth()
        {
            maxHealth = progression.baseHealth;
            maxHealth += level * progression.healthGainedPerLevel;
            currentHealth = maxHealth;
        }

        public float GetHealthFraction()
        {
            return currentHealth / maxHealth;
        }

        private void SetDamage()
        {
            float addedDamage = level * progression.damageGainedPerLevel;
            damage = progression.baseDamage + addedDamage;
        }

        public float GetDamage()
        {
            return damage;
        }

        public event Action OnHealthChange;
        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            OnHealthChange();
        }
    }
}