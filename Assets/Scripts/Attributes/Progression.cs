using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Attributes
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Character/Create Character Progression", order = 2)]
    public class Progression : ScriptableObject
    {
        [FoldoutGroup("Health")]
        public float baseHealth = 150;
        [FoldoutGroup("Health")]
        public float healthGainedPerLevel = 50;

        [FoldoutGroup("Damage")]
        public float baseDamage = 20;
        [FoldoutGroup("Damage")]
        public float damageGainedPerLevel = 5;
    }
}