using Sirenix.OdinInspector;

namespace RPG.Combat
{
    public enum AOEPackageEnum
    {
        None,
        Wait,
        Executable
    }

    [System.Serializable]
    public class AOEPackageItem
    {
        public AOEPackageEnum aoePackageEnum;

        [ShowIf("aoePackageEnum", AOEPackageEnum.Executable)]
        public EffectPackage effectPackage;

        [ShowIf("aoePackageEnum", AOEPackageEnum.Wait)]
        public float duration;
    }
}