using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public class EffectItem
    {
        public EffectEnum effectEnum;

        [ShowIf("effectEnum", EffectEnum.Wait)]
        public float duration;

        [ShowIf("effectEnum", EffectEnum.Damage)]
        public LayerMask layerToAffect;
        [ShowIf("effectEnum", EffectEnum.Damage)]
        public float value;
    }
}