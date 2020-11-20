using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Core
{
    public abstract class SkillScriptableObject : ScriptableObject
    {
        public string skillName = "New Skill";
        public Sprite skillSprite;
        public GameObject skillVFX;
        public AudioClip skillVocalAudio;
        public AudioClip skillActionAudio;
        public float skillBaseCooldown = 3f;

        [Header("Pick Only One Aim Mechanic If Necessary")]
        public bool skillUsesSkillShot;
        [ShowIf("skillUsesSkillShot")]
        public Sprite skillShotImage;

        [Space]
        public bool skillUsesRangeShot;
        [ShowIf("skillUsesRangeShot")]
        public Sprite skillRangeImage;
        [ShowIf("skillUsesRangeShot")]
        public Sprite skillReticleImage;

        public abstract void Initialize(GameObject obj, Animator animator, string str);
        public abstract void TriggerSkill();
    }
}