using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats = null;
        [SerializeField] RectTransform foreground = null;

        private void Start()
        {
            baseStats.OnHealthChange += UpdateHealthBar;
        }

        public void UpdateHealthBar()
        {
            float fraction = baseStats.GetHealthFraction();
            foreground.localScale = new Vector3(fraction, 1, 1);
        }
    }
}