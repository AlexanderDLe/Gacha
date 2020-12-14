using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    public class AutoAttack_SO : ScriptableObject
    {
        public Weapon weapon;

        [InfoBox("The number of the attack hits MUST BE THE SAME as the array length of the damage fractions and hit radiuses!")]
        public int numberOfAutoAttackHits;
        public EffectPackage[] effectPackage;

        [FoldoutGroup("FX")]
        public GameObject[] autoAttackVFX;
        [FoldoutGroup("FX")]
        public AudioClip[] weakAttackAudio;
        [FoldoutGroup("FX")]
        public AudioClip[] mediumAttackAudio;
    }
}