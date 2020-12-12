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
        public AOEInvoker aoeInvoker;

        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, AOEInvoker aoeCreator)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
            this.raycaster = raycaster;
            this.animator = animator;
            this.aoeInvoker = aoeCreator;
        }

        public override void Initialize(BaseStats baseStats, PlayableCharacter_SO script)
        {
            this.baseStats = baseStats;
            this.script = script;
            InitializeMovementSkill();
            InitializePrimarySkill();
            InitializeUltimateSkill();
        }

        public override void InitializeSkillManager(SkillManager movementSkill, SkillManager primarySkill, SkillManager ultimateSkill)
        {
            this.movementSkill = movementSkill;
            this.primarySkill = primarySkill;
            this.ultimateSkill = ultimateSkill;
        }

        #region Movement Skill
        SkillManager movementSkill;
        MovementSkill movementScript;

        public override void InitializeMovementSkill()
        {
            /*  Cast Skill_SO as MovementSkill:
                MovementSkill inherits from abstract class Skill_SO */
            this.movementScript = script.movementSkill as MovementSkill;
            objectPooler.AddToPool(movementScript.skillPrefab, movementScript.poolCount);
        }
        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
        public void MCMovementStart()
        {
            FXObject fxObj = objectPooler.SpawnFromPool(movementScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position, transform.rotation, movementScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, movementScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movementScript.skillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        SkillManager primarySkill;
        ProjectileSkill primarySkillScript;
        Projectile_SO primaryProjectile;
        Vector3 primaryProjDestination = Vector3.zero;

        public override void InitializePrimarySkill()
        {
            /*  Cast Skill_SO as ProjectileSkill:
                ProjectileSkill inherits from abstract class Skill_SO */
            this.primarySkillScript = script.primarySkill as ProjectileSkill;
            this.primaryProjectile = primarySkillScript.projectile_SO;

            objectPooler.AddToPool(primarySkillScript.projectile_SO.prefab, 5);
        }
        public override void TriggerPrimarySkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint(LayerMask.GetMask("Terrain"));
            primaryProjDestination = hit.point;

            audioManager.PlayAudio(AudioEnum.Character, primarySkillScript.skillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject, hit);
            animator.SetTrigger("primarySkill");
        }

        public void MCPrimaryTriggered()
        {
            string prefabName = primaryProjectile.prefab.name;
            LayerMask layerToharm = LayerMask.GetMask("Enemy");
            Vector3 origin = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 destination = primaryProjDestination;

            float damage = 20f;
            float speed = primaryProjectile.speed;
            float lifetime = primaryProjectile.maxLifeTime;
            float activeTime = primaryProjectile.activeTime;
            bool hasActiveTime = primaryProjectile.hasActiveTime;

            Projectile proj = objectPooler.SpawnFromPool(prefabName).GetComponent<Projectile>();
            proj.Initialize(origin, destination, speed, damage, lifetime, layerToharm, hasActiveTime, activeTime);

            audioManager.PlayAudio(AudioEnum.Action, primarySkillScript.skillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        SkillManager ultimateSkill;
        AOESkill ultimateScript;

        public override void InitializeUltimateSkill()
        {
            /* Cast Skill_SO as AOESkill: 
                AOESkill inherits from abstract class Skill_SO */
            this.ultimateScript = script.ultimateSkill as AOESkill;
            objectPooler.AddToPool(ultimateScript.skillPrefab, ultimateScript.poolCount);
        }
        public override void TriggerUltimateSkill()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateScript.skillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void MCUltimateTriggered()
        {
            FXObject aoeObj = objectPooler.SpawnFromPool(ultimateScript.skillPrefab.name).GetComponent<FXObject>();

            aoeObj.Initialize(transform.position, transform.rotation, ultimateScript.lifetime);
            aoeInvoker.Invoke(aoeObj.aoeCenterPoint.position, 12f, LayerMask.GetMask("Enemy"), 20f);

            audioManager.PlayAudio(AudioEnum.Action, ultimateScript.skillActionAudio);
        }
        #endregion
    }
}