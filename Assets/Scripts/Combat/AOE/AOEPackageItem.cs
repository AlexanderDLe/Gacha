using Sirenix.OdinInspector;

namespace RPG.Combat
{
    [System.Serializable]
    public struct AOEPackageItem
    {
        [GUIColor(.5f, .9f, 1f, 1f)]
        public AOEPackageEnum aoePackageEnum;

        [ShowIf("aoePackageEnum", AOEPackageEnum.Executable)]
        public EffectPackage effectPackage;

        [ShowIf("aoePackageEnum", AOEPackageEnum.Wait)]
        public float duration;
    }
}