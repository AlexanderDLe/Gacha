using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using RPG.Skill;
using UnityEngine;

namespace RPG.Characters
{
    public class MCEventHandler : SkillEventHandler
    {
        public AudioManager audioManager;
        public BaseStats baseStats;
        public PlayableCharacter_SO script;
        public ObjectPooler objectPooler;
        public RaycastMousePosition raycaster;
        public Animator animator;

        public MeleeAttacker meleeAttacker = null;
        public ProjectileLauncher projectileLauncher = null;
        public AOECaster aoeCaster = null;

        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, MeleeAttacker meleeAttacker, ProjectileLauncher projectileLauncher, AOECaster aoeCaster)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
            this.raycaster = raycaster;
            this.animator = animator;

            this.meleeAttacker = meleeAttacker;
            this.projectileLauncher = projectileLauncher;
            this.aoeCaster = aoeCaster;
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

        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
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
        Vector3 primaryProjDestination = Vector3.zero;

        public override void TriggerPrimarySkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint(LayerMask.GetMask("Terrain"));
            primaryProjDestination = hit.point;

            audioManager.PlayAudio(AudioEnum.Character, primarySkillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject, hit);
            animator.SetTrigger("primarySkill");
        }
        public void MCPrimaryTriggered()
        {
            string prefabName = primaryProjectile.prefab.name;
            Vector3 origin = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 destination = primaryProjDestination;
            float speed = primaryProjectile.speed;
            float damage = 20f;
            float lifetime = primaryProjectile.maxLifeTime;
            string layerToharm = "Enemy";
            bool hasActiveTime = primaryProjectile.hasActiveTime;
            float activeTime = primaryProjectile.activeTime;

            projectileLauncher.Shoot(prefabName, origin, destination, speed, damage, lifetime, layerToharm, hasActiveTime, activeTime);

            // Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;

        public override void TriggerUltimateSkill()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void MCUltimateTriggered()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }
        #endregion
    }
}