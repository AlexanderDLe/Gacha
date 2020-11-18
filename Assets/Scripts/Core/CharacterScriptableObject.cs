using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Character", order = 0)]
    public class CharacterScriptableObject : ScriptableObject
    {
        [Title("Character")]
        public string characterName;
        public GameObject characterPrefab;
        public AnimatorOverrideController animatorOverride;
        public AudioClip[] dashAudio;

        [Title("Auto Attack")]
        public int numberOfAutoAttackHits;
        [Header("Auto Attack FX")]
        public GameObject[] autoAttackVFX;
        public AudioClip[] weakAttackAudio;
        public AudioClip[] mediumAttackAudio;

        [Title("Movement Skill")]
        public float movementSkillCooldownTime;
        public GameObject movementSkillVFX;
        public AudioClip movementSkillActionAudio;
        public AudioClip movementSkillVocalAudio;

        [Title("Primary Skill")]
        public float primarySkillCooldownTime;
        [Header("Pick One Aim Mechanic At Most")]

        public bool primaryUsesSkillShot;
        [ShowIf("primaryUsesSkillShot")]
        public Sprite primarySkillShotImage;

        [Space]
        public bool primaryUsesRangeShot;
        [ShowIf("primaryUsesRangeShot")]
        public Sprite primarySkillRangeImage;
        [ShowIf("primaryUsesRangeShot")]
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