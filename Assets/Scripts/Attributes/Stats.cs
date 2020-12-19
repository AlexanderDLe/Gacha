using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.Attributes
{
    public class Stats : MonoBehaviour
    {
        [Range(1, 50)]
        public int level = 1;
        public Progression progression = null;
        public float maxHealth = 100f;
        public float currentHealth = 100f;
        public float damage = 5f;
        public float defense = 5f;
        public float movementSpeed = 5f;

        public void Initialize(BaseCharacter_SO charScript)
        {
            this.progression = charScript.progression;
            SetHealth();
            SetDamage();
            SetAttributes(charScript);
        }

        private void SetHealth()
        {
            maxHealth = progression.baseHealth;
            maxHealth += level * progression.healthGainedPerLevel;
            currentHealth = maxHealth;
        }

        private void SetAttributes(BaseCharacter_SO script)
        {
            movementSpeed = script.movementSpeed;
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