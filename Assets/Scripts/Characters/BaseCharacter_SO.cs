using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Characters
{
    public abstract class BaseCharacter_SO : ScriptableObject
    {
        public new string name;
        [FoldoutGroup("Metadata")]
        public Progression progression;
        [FoldoutGroup("Metadata")]
        public GameObject prefab;
        [FoldoutGroup("Metadata")]
        public Sprite image;
        [FoldoutGroup("Metadata")]
        public Avatar characterAvatar;
        [FoldoutGroup("Metadata")]
        public RuntimeAnimatorController animatorController;
        [FoldoutGroup("Metadata")]
        public AnimatorOverrideController animatorOverride;

        [FoldoutGroup("Attributes")]
        public float movementSpeed;
    }
}