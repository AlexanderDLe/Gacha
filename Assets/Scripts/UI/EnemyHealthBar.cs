using UnityEngine;

namespace RPG.Attributes
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] Stats stats = null;
        [SerializeField] RectTransform foreground = null;

        private void OnEnable()
        {
            stats.OnHealthChange += UpdateHealthBar;
        }
        private void OnDisable()
        {
            stats.OnHealthChange -= UpdateHealthBar;
        }

        public void UpdateHealthBar()
        {
            float fraction = stats.GetHealthFraction();
            foreground.localScale = new Vector3(fraction, 1, 1);
        }
    }
}