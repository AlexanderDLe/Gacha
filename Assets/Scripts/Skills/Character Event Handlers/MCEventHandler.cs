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

        public AOECreator aoeCreator = null;
        public ProjectileLauncher projectileLauncher = null;

        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, AOECreator aoeCreator, ProjectileLauncher projectileLauncher)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
            this.raycaster = raycaster;
            this.animator = animator;

            this.aoeCreator = aoeCreator;
            this.projectileLauncher = projectileLauncher;
        }

        public override void Initialize(BaseStats baseStats, PlayableCharacter_SO script)
        {
            this.baseStats = baseStats;
            this.script = script;
            InitializeMovementSkill();
            InitializePrimarySkill();
            InitializeUltimateSkill();
        }

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;

        public override void InitializeMovementSkill()
        {
            this.movementSkillVFX = script.movementSkill.skillPrefab;
            this.movementSkillVocalAudio = script.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = script.movementSkill.skillActionAudio;
        }
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

        private void InitializePrimarySkill()
        {
            /* Cast Skill_SO as ProjectileSkill 
            ProjectileSkill inherits from abstract class Skill_SO */
            this.primarySkill = script.primarySkill as ProjectileSkill;
            this.primaryProjectile = primarySkill.projectile_SO;

            objectPooler.AddToPool(primarySkill.projectile_SO.prefab, 5);

            this.primarySkillVFX = primarySkill.skillPrefab;
            this.primarySkillVocalAudio = primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = primarySkill.skillActionAudio;
        }
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
            LayerMask layerToharm = LayerMask.GetMask("Enemy");
            Vector3 origin = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 destination = primaryProjDestination;
            float speed = primaryProjectile.speed;
            float damage = 20f;
            float lifetime = primaryProjectile.maxLifeTime;
            bool hasActiveTime = primaryProjectile.hasActiveTime;
            float activeTime = primaryProjectile.activeTime;

            projectileLauncher.Shoot(prefabName, origin, destination, speed, damage, lifetime, layerToharm, hasActiveTime, activeTime);

            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        AOESkill ultimateSkill;

        private void InitializeUltimateSkill()
        {
            this.ultimateSkill = script.ultimateSkill as AOESkill;

            objectPooler.AddToPool(ultimateSkill.skillPrefab, 2);

            this.ultimateSkillVFX = script.ultimateSkill.skillPrefab;
            this.ultimateSkillVocalAudio = script.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = script.ultimateSkill.skillActionAudio;
        }
        public override void TriggerUltimateSkill()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void MCUltimateTriggered()
        {
            AOEObject aoeObj = objectPooler.SpawnFromPool(ultimateSkillVFX.name).GetComponent<AOEObject>();

            aoeObj.Initialize(transform.position, transform.rotation, 3);
            aoeCreator.Invoke(aoeObj.aoeCenterPoint.position, 10f, LayerMask.GetMask("Enemy"), 20f);

            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }
        #endregion
    }
}