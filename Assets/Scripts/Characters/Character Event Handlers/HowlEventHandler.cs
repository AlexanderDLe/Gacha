using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class HowlEventHandler : SkillEventHandler
    {
        public override void LinkReferences(AudioManager audioManager, ObjectPooler charObjectPooler, RaycastMousePosition raycaster, Animator animator, NavMeshAgent navMeshAgent, ProjectileLauncher projectileLauncher)
        {
            this.audioManager = audioManager;
            this.charObjectPooler = charObjectPooler;
            this.raycaster = raycaster;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.projectileLauncher = projectileLauncher;
        }
        public override void Initialize(Stats stats, PlayableCharacter_SO script)
        {
            this.stats = stats;
            this.script = script;
            InitializeMovementSkill();
            InitializePrimarySkill();
            InitializeUltimateSkill();
        }

        public override void InitializeSkillManagers(SkillManager movementSkill, SkillManager primarySkill, SkillManager ultimateSkill)
        {
            this.movSkill = movementSkill;
            this.priSkill = primarySkill;
            this.ultSkill = ultimateSkill;
            InitializeDictionaries();
        }

        #region Interfacing
        private void InitializeDictionaries()
        {
            this.enterSkillDict = new Dictionary<string, Action>() {
                { "movementSkill", EnterMovementSkill},
                { "primarySkill", EnterPrimarySkill},
                { "ultimateSkill", EnterUltimateSkill},
            };
            this.executeSkillDict = new Dictionary<string, Action>() {
                { "movementSkill", ExecuteMovementSkill},
                { "primarySkill", ExecutePrimarySkill},
                { "ultimateSkill", ExecuteUltimateSkill},
            };
            this.exitSkillDict = new Dictionary<string, Action>() {
                { "movementSkill", ExitMovementSkill},
                { "primarySkill", ExitPrimarySkill},
                { "ultimateSkill", ExitUltimateSkill},
            };
        }
        #endregion

        #region Movement Skill
        MovementSkill movScript;

        public override void InitializeMovementSkill()
        {
            movScript = script.movementSkill as MovementSkill;
            charObjectPooler.AddToPool(movScript.skillPrefab, movScript.poolCount);
        }
        public override void EnterMovementSkill()
        {
            movSkill.BeginSkillCooldown();
            navMeshAgent.updateRotation = false;

            EffectObject fxObj = charObjectPooler.SpawnFromPool(movScript.skillPrefab.name).GetComponent<EffectObject>();

            fxObj.Initialize(transform.position, transform.rotation, movScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, movScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movScript.skillVocalAudio, probability);
        }
        public override void ExecuteMovementSkill()
        {
            Vector3 movement;
            movement = gameObject.transform.forward;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = 10000f;
            navMeshAgent.destination = gameObject.transform.position + movement;
        }
        public override void ExitMovementSkill()
        {
            navMeshAgent.updateRotation = true;
        }
        #endregion

        #region Primary Skill
        AOESkill priScript;

        public override void InitializePrimarySkill()
        {
            this.priScript = script.primarySkill as AOESkill;
            charObjectPooler.AddToPool(priScript.skillPrefab, priScript.poolCount);
        }
        public override void EnterPrimarySkill()
        {
            priSkill.BeginSkillCooldown();

            audioManager.PlayAudio(AudioEnum.Character, priScript.skillVocalAudio);

            EffectObject fxObj = charObjectPooler.SpawnFromPool(priScript.skillPrefab.name).GetComponent<EffectObject>();
            AOEEffect aoeFX = fxObj.GetComponent<AOEEffect>();

            fxObj.Initialize(transform.position + transform.forward * 3, transform.rotation, priScript.lifetime);
            aoeFX.Initialize(priScript, gameObject);
        }
        public void HowlPrimaryTriggered()
        {
            audioManager.PlayAudio(AudioEnum.Action, priScript.skillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        AOESkill ultScript;

        public override void InitializeUltimateSkill()
        {
            this.ultScript = script.ultimateSkill as AOESkill;
            charObjectPooler.AddToPool(ultScript.skillPrefab, ultScript.poolCount);
        }
        public override void EnterUltimateSkill()
        {
            ultSkill.BeginSkillCooldown();
        }
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultScript.skillVocalAudio);
        }
        public void HowlUltimateTriggered()
        {
            EffectObject fxObj = charObjectPooler.SpawnFromPool(ultScript.skillPrefab.name).GetComponent<EffectObject>();

            fxObj.Initialize(transform.position, transform.rotation, ultScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, ultScript.skillActionAudio);
        }
        #endregion
    }
}