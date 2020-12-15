using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Attributes;
using RPG.Combat;

namespace RPG.Characters
{
    public abstract class BaseCharacter_SO : ScriptableObject
    {
        [FoldoutGroup("Metadata")]
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

        [FoldoutGroup("Attributes")]
        public float movementSpeed;
    }
}