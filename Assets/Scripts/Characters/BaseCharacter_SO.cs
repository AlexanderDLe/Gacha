using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Characters
{
    public abstract class BaseCharacter_SO : ScriptableObject
    {
        #region Character Config
        [Title("Character Config")]
        public Progression progression;
        public new string name;
        [FoldoutGroup("Metadata")]
        public GameObject prefab;
        [FoldoutGroup("Metadata")]
        public Sprite image;
        [FoldoutGroup("Metadata")]
        public Avatar characterAvatar;
        [FoldoutGroup("Metadata")]
        public AnimatorOverrideController animatorOverride;
        [FoldoutGroup("Attributes")]
        public float health;
        [FoldoutGroup("Attack")]
        public float baseDamage;
        #endregion
    }
}