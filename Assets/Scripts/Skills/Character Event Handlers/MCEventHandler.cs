using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Skill;
using UnityEngine;

namespace RPG.Characters
{
    public class MCEventHandler : AnimationEventHandler
    {
        public AudioManager audioManager;
        public BaseStats baseStats;
        public PlayableCharacter_SO script;
        public ProjectileLauncher projectileLauncher;
        public ObjectPooler objectPooler;

        private void Awake()
        {
            projectileLauncher = GetComponent<ProjectileLauncher>();
        }
        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
        }

        public override void Initialize(BaseStats baseStats, PlayableCharacter_SO script)
        {
            this.baseStats = baseStats;
            this.script = script;
            InitializeSkills();
            InitializePrimarySkill();
        }

        public override void InitializeSkills()
        {
            this.movementSkillVFX = script.movementSkill.skillVFX;
            this.movementSkillVocalAudio = script.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = script.movementSkill.skillActionAudio;

            this.ultimateSkillVFX = script.ultimateSkill.skillVFX;
            this.ultimateSkillVocalAudio = script.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = script.ultimateSkill.skillActionAudio;
        }

        private void InitializePrimarySkill()
        {
            /* Cast Skill_SO as ProjectileSkill 
            ProjectileSkill inherits from abstract class Skill_SO */
            this.primarySkill = script.primarySkill as ProjectileSkill;
            this.primaryProjectile = primarySkill.projectile_SO;

            objectPooler.AddToPool(primarySkill.projectile_SO.prefab, 3);

            this.primarySkillVFX = primarySkill.skillVFX;
            this.primarySkillVocalAudio = primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = primarySkill.skillActionAudio;
        }

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;

        public void MCMovementStart()
        {
            Instantiate(movementSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, movementSkillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movementSkillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        GameObject primarySkillVFX = null;
        AudioClip primarySkillActionAudio = null;
        AudioClip primarySkillVocalAudio = null;
        ProjectileSkill primarySkill;
        Projectile_SO primaryProjectile;

        public void MCPrimaryStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, primarySkillVocalAudio);
        }
        public void MCPrimaryTrigger()
        {
            string prefabName = primaryProjectile.prefab.name;
            Vector3 origin = transform.position;
            Vector3 destination = transform.position + transform.forward;
            float speed = primaryProjectile.speed;
            float damage = 20f;
            float lifetime = primaryProjectile.maxLifeTime;
            string layerToharm = "Enemy";

            projectileLauncher.Shoot(prefabName, origin, destination, speed, damage, lifetime, layerToharm);

            // Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        public void MCUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
        }
        public void MCUltimateTrigger()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }
        #endregion
    }
}