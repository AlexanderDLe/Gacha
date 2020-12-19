using Sirenix.OdinInspector;

namespace RPG.Combat
{
    [System.Serializable]
    public struct EffectItem
    {
        public E_Enum effectEnum;

        [ShowIf("effectEnum", E_Enum.Wait)]
        public float duration;

        [ShowIf("effectEnum", E_Enum.Damage)]
        public DamagePacket damagePacket;
    }

    [System.Serializable]
    public struct DamagePacket
    {
        public float physDmgFraction;
        public float fireDmgFraction;
    }
}