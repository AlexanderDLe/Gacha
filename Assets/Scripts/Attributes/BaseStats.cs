using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

namespace RPG.Attributes
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 50)]
        [SerializeField] int level = 1;
        [SerializeField] float maxHealth = 100f;
        [SerializeField] float currHealth = 100f;
        [SerializeField] Progression progression = null;

        public void Initialize(BaseCharacter_SO charScript)
        {
            this.progression = charScript.progression;
            SetHealth();
        }

        private void SetHealth()
        {
            maxHealth = progression.baseHealth;
            maxHealth += level * progression.healthGainedPerLevel;
            currHealth = maxHealth;
        }
    }
}