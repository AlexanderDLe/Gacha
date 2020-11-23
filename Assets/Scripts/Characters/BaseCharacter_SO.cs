using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class BaseCharacter_SO : ScriptableObject
    {
        #region Character Config
        [Title("Character Config")]
        public new string name;
        public float health;
        public GameObject prefab;
        public Sprite image;
        public Avatar characterAvatar;
        public AnimatorOverrideController animatorOverride;
        #endregion
    }
}