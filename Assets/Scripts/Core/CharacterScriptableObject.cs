using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Character", order = 0)]
    public class CharacterScriptableObject : ScriptableObject
    {
        #region Character Config
        [Title("Character Config")]
        public string characterName;
        public float characterHealth;
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

        public SkillScriptableObject movementSkill;
        public SkillScriptableObject primarySkill;
        public SkillScriptableObject ultimateSkill;
    }
}