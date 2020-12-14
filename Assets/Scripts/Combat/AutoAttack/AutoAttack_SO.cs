using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class AutoAttack_SO : ScriptableObject
    {
        [FoldoutGroup("Attack")]
        [InfoBox("The number of the attack hits MUST BE THE SAME as the array length of the effect packages and hit radiuses!")]
        public int numberOfAutoAttackHits;
        [FoldoutGroup("Attack")]
        public EffectPackage[] effectPackages;

        [FoldoutGroup("FX")]
        public GameObject[] autoAttackVFX;
        [FoldoutGroup("FX")]
        public AudioClip[] weakAttackAudio;
        [FoldoutGroup("FX")]
        public AudioClip[] mediumAttackAudio;
    }
}