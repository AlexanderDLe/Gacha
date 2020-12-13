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
            this.movSkill = movementSkill;
            this.priSkill = primarySkill;
            this.ultSkill = ultimateSkill;
        }

        #region Movement Skill
        SkillManager movSkill;
        MovementSkill movScript;

        public override void InitializeMovementSkill()
        {
            /*  Cast Skill_SO as MovementSkill:
                MovementSkill inherits from abstract class Skill_SO */
            this.movScript = script.movementSkill as MovementSkill;
            objectPooler.AddToPool(movScript.skillPrefab, movScript.poolCount);
        }
        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
        public void MCMovementStart()
        {
            EffectObject fxObj = objectPooler.SpawnFromPool(movScript.skillPrefab.name).GetComponent<EffectObject>();

            fxObj.Initialize(transform.position, transform.rotation, movScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, movScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movScript.skillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        SkillManager priSkill;
        ProjectileSkill priScript;
        Projectile_SO primaryProjectile;
        Vector3 primaryProjDestination = Vector3.zero;

        public override void InitializePrimarySkill()
        {
            /*  Cast Skill_SO as ProjectileSkill:
                ProjectileSkill inherits from abstract class Skill_SO */
            this.priScript = script.primarySkill as ProjectileSkill;
            this.primaryProjectile = priScript.projectile_SO;

            objectPooler.AddToPool(priScript.projectile_SO.prefab, 5);
        }
        public override void TriggerPrimarySkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint(LayerMask.GetMask("Terrain"));
            primaryProjDestination = hit.point;

            audioManager.PlayAudio(AudioEnum.Character, priScript.skillVocalAudio);
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

            audioManager.PlayAudio(AudioEnum.Action, priScript.skillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        SkillManager ultSkill;
        AOESkill ultScript;

        public override void InitializeUltimateSkill()
        {
            /* Cast Skill_SO as AOESkill: 
                AOESkill inherits from abstract class Skill_SO */
            this.ultScript = script.ultimateSkill as AOESkill;
            objectPooler.AddToPool(ultScript.skillPrefab, ultScript.poolCount);
        }
        public override void TriggerUltimateSkill()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultScript.skillVocalAudio);
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void MCUltimateTriggered()
        {
            EffectObject fxObj = objectPooler.SpawnFromPool(ultScript.skillPrefab.name).GetComponent<EffectObject>();
            AOEEffect aoeObj = fxObj.GetComponent<AOEEffect>();

            fxObj.Initialize(transform.position, transform.rotation, ultScript.lifetime);
            aoeObj.Initialize(ultScript, gameObject);

            audioManager.PlayAudio(AudioEnum.Action, ultScript.skillActionAudio);
        }
        #endregion
    }
}