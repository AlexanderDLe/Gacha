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
        public GameObject[] autoAttackVFX;
        public AudioClip[] weakAttackAudio;
        public AudioClip[] mediumAttackAudio;

        [Header("Primary Skill")]
        public bool useSkillShot;
        public bool useRangeShot;
        public GameObject primarySkillFX;
        public AudioClip[] primarySkillAudio;

        /* [Header("Audio Clips")]
        public AudioClipClass[] AudioClips = null;

        [System.Serializable]
        public class AudioClipClass
        {
            public string stat;
            public AudioClip[] levels;
        } */
    }
}