using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Characters
{
    public abstract class BaseCharacter_SO : ScriptableObject
    {
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
    }
}