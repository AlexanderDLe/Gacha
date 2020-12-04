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
        public float baseCooldownTime = 3f;

        [Header("Pick Only One Aim Mechanic If Necessary")]
        public bool requiresSkillShot;
        [ShowIf("requiresSkillShot")]
        public Sprite skillShotImage;

        [Space]
        public bool requiresRangeShot;
        [ShowIf("requiresRangeShot")]
        public Sprite skillRangeImage;
        [ShowIf("requiresRangeShot")]
        public Sprite skillReticleImage;

        public abstract void Initialize(GameObject playerGameObject, string str);

        public abstract void TriggerSkill(string skillType);
    }
}