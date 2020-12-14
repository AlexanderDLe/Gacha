using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Skill
{
    public abstract class Skill_SO : ScriptableObject
    {
        [FoldoutGroup("Metadata")]
        public string skillName = "New Skill";
        [FoldoutGroup("Metadata")]
        public Sprite skillSprite;
        [FoldoutGroup("Metadata")]
        public bool debug;
        [FoldoutGroup("Metadata")]
        public int poolCount = 3;

        [FoldoutGroup("Timing")]
        public float baseCooldownTime = 3f;
        [FoldoutGroup("Timing")]
        public float lifetime = 3f;

        [FoldoutGroup("Skill FX")]
        public GameObject skillPrefab;
        [FoldoutGroup("Skill FX")]
        public AudioClip skillVocalAudio;
        [FoldoutGroup("Skill FX")]
        public AudioClip skillActionAudio;

        [FoldoutGroup("Aim Mechanics")]
        public bool requiresSkillShot;
        [ShowIf("requiresSkillShot"), FoldoutGroup("Aim Mechanics")]
        public Sprite skillShotImage;

        [FoldoutGroup("Aim Mechanics")]
        public bool requiresRangeShot;
        [ShowIf("requiresRangeShot"), FoldoutGroup("Aim Mechanics")]
        public Sprite skillRangeImage;
        [ShowIf("requiresRangeShot"), FoldoutGroup("Aim Mechanics")]
        public Sprite skillReticleImage;
    }
}