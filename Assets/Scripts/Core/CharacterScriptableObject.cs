using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Character", order = 0)]
    public class CharacterScriptableObject : ScriptableObject
    {
        #region Character Config
        [Title("Character Config")]
        public string characterName;
        public GameObject characterPrefab;
        public AnimatorOverrideController animatorOverride;
        #endregion

        #region Character FX
        [FoldoutGroup("Character FX")]
        public AudioClip[] dashAudio;
        #endregion

        #region Auto Attack
        [FoldoutGroup("Auto Attack")]
        public int numberOfAutoAttackHits;
        [Header("Auto Attack FX")]
        [FoldoutGroup("Auto Attack")]
        public GameObject[] autoAttackVFX;
        [FoldoutGroup("Auto Attack")]
        public AudioClip[] weakAttackAudio;
        [FoldoutGroup("Auto Attack")]
        public AudioClip[] mediumAttackAudio;
        #endregion

        #region Movement Skill
        [FoldoutGroup("Movement Skill")]
        public float movementSkillCooldownTime;
        [FoldoutGroup("Movement Skill")]
        public GameObject movementSkillVFX;
        [FoldoutGroup("Movement Skill")]
        public AudioClip movementSkillActionAudio;
        [FoldoutGroup("Movement Skill")]
        public AudioClip movementSkillVocalAudio;
        #endregion

        #region Primary Skill
        [FoldoutGroup("Primary Skill")]
        public float primaryCooldownResetTime;

        [Header("Pick One Aim Mechanic At Most")]
        [FoldoutGroup("Primary Skill")]
        public bool primaryUsesSkillShot;
        [FoldoutGroup("Primary Skill"), ShowIf("primaryUsesSkillShot")]
        public Sprite primarySkillShotImage;

        [Space]
        [FoldoutGroup("Primary Skill")]
        public bool primaryUsesRangeShot;
        [FoldoutGroup("Primary Skill"), ShowIf("primaryUsesRangeShot")]
        public Sprite primaryRangeImage;
        [FoldoutGroup("Primary Skill"), ShowIf("primaryUsesRangeShot")]
        public Sprite primaryReticleImage;

        [Space]
        [Header("Primary FX")]
        [FoldoutGroup("Primary Skill")]
        public GameObject primarySkillVFX;
        [FoldoutGroup("Primary Skill")]
        public AudioClip primarySkillActionAudio;
        [FoldoutGroup("Primary Skill")]
        public AudioClip primarySkillVocalAudio;
        #endregion


        #region Ultimate Skill
        [FoldoutGroup("Ultimate Skill")]
        public float ultimateCooldownResetTime;

        [Header("Pick One Aim Mechanic At Most")]
        [FoldoutGroup("Ultimate Skill")]
        public bool ultimateUsesSkillShot;
        [FoldoutGroup("Ultimate Skill"), ShowIf("ultimateUsesSkillShot")]
        public Sprite ultimateSkillShotImage;

        [Space]
        [FoldoutGroup("Ultimate Skill")]
        public bool ultimateUsesRangeShot;
        [FoldoutGroup("Ultimate Skill"), ShowIf("ultimateUsesRangeShot")]
        public Sprite ultimateRangeImage;
        [FoldoutGroup("Ultimate Skill"), ShowIf("ultimateUsesRangeShot")]
        public Sprite ultimateReticleImage;

        [Space]
        [Header("Ultimate FX")]
        [FoldoutGroup("Ultimate Skill")]
        public GameObject ultimateSkillVFX;
        [FoldoutGroup("Ultimate Skill")]
        public AudioClip ultimateSkillActionAudio;
        [FoldoutGroup("Ultimate Skill")]
        public AudioClip ultimateSkillVocalAudio;
        #endregion
    }
}