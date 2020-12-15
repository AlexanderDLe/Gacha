using Sirenix.OdinInspector;

namespace RPG.Combat
{
    [System.Serializable]
    public struct EffectItem
    {
        public IE_Enum effectEnum;

        [ShowIf("effectEnum", IE_Enum.Wait)]
        public float duration;

        [ShowIf("effectEnum", IE_Enum.Damage)]
        public float value;
    }
}