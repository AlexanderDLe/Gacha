using RPG.Combat;
using RPG.Core;
using RPG.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Playable Character", order = 0)]
    public class PlayableCharacter_SO : BaseCharacter_SO
    {
        [FoldoutGroup("Metadata")]
        public PlayableCharEnum characterEnum;

        #region Character FX
        [FoldoutGroup("Character FX")]
        public AudioClip[] dashAudio;
        #endregion

        #region Auto Attack
        [FoldoutGroup("Auto Attack")]
        public FightTypeEnum fightingType;
        [FoldoutGroup("Auto Attack")]
        public Weapon weapon;
        [FoldoutGroup("Auto Attack"), ShowIf("fightingType", FightTypeEnum.Projectile)]
        public Projectile_SO projectile;

        [InfoBox("The number of the attack hits MUST BE THE SAME as the array length of the damage fractions and hit radiuses!")]
        [FoldoutGroup("Auto Attack")]
        public int numberOfAutoAttackHits;
        [FoldoutGroup("Auto Attack")]
        public float[] autoAttackDamageFractions;
        [FoldoutGroup("Auto Attack"), ShowIf("fightingType", FightTypeEnum.Melee)]
        public float[] autoAttackHitRadiuses;

        [Header("Auto Attack FX")]
        [FoldoutGroup("Auto Attack")]
        public GameObject[] autoAttackVFX;
        [FoldoutGroup("Auto Attack")]
        public AudioClip[] weakAttackAudio;
        [FoldoutGroup("Auto Attack")]
        public AudioClip[] mediumAttackAudio;
        #endregion

        #region Skills
        [FoldoutGroup("Skills")]
        public Skill_SO movementSkill;
        [FoldoutGroup("Skills")]
        public Skill_SO primarySkill;
        [FoldoutGroup("Skills")]
        public Skill_SO ultimateSkill;
        #endregion
    }
}