using Sirenix.OdinInspector;

namespace RPG.Combat
{
    [System.Serializable]
    public class Effect
    {
        public EffectEnum effectEnum;

        [ShowIf("effectEnum", EffectEnum.Damage)]
        public float value;

        [ShowIf("effectEnum", EffectEnum.Wait)]
        public float duration;
    }
}