﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Skill
{
    public abstract class Skill_SO : ScriptableObject
    {
        public string skillName = "New Skill";
        public Sprite skillSprite;
        public float baseCooldownTime = 3f;

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