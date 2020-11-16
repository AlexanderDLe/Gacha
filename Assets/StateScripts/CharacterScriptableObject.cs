using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Character", order = 0)]
    public class CharacterScriptableObject : ScriptableObject
    {
        public string characterName;
        public GameObject characterPrefab;
        public AnimatorOverrideController animatorOverride;

        [Header("Character")]
        public AudioClip[] dashAudio;

        [Header("Auto Attack")]
        public int numberOfAutoAttackHits;
        public GameObject[] autoAttackFX;
        public AudioClip[] weakAttackAudio;
        public AudioClip[] mediumAttackAudio;

        [Header("Primary Skill")]
        public bool useSkillShot;
        public bool useRangeShot;
        public GameObject primarySkillFX;
        public AudioClip[] primarySkillAudio;
    }
}