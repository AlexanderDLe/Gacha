using UnityEngine;
using UnityEngine.UI;

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
        [Header("Auto Attack FX")]
        public GameObject[] autoAttackVFX;
        public AudioClip[] weakAttackAudio;
        public AudioClip[] mediumAttackAudio;

        [Header("Movement Skill")]
        public float movementSkillCooldownTime;
        public GameObject movementSkillVFX;
        public AudioClip movementSkillActionAudio;
        public AudioClip movementSkillVocalAudio;

        [Header("Primary Skill")]
        public float primarySkillCooldownTime;
        [Header("Pick One Aim Mechanic At Most")]
        public bool primaryUsesSkillShot;
        public Sprite primarySkillShotImage;

        [Space]
        public bool primaryUsesRangeShot;
        public Sprite primarySkillRangeImage;
        public Sprite primarySkillReticleImage;

        [Space]
        [Header("Primary FX")]
        public GameObject primarySkillVFX;
        public AudioClip primarySkillActionAudio;
        public AudioClip primarySkillVocalAudio;

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