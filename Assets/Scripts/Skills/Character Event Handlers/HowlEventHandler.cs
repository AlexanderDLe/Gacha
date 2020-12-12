using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using RPG.Skill;
using UnityEngine;

namespace RPG.Characters
{
    public class HowlEventHandler : SkillEventHandler
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
            movementScript = script.movementSkill as MovementSkill;
            objectPooler.AddToPool(movementScript.skillPrefab, movementScript.poolCount);
        }
        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
        public void HowlMovementStart()
        {
            FXObject fxObj = objectPooler.SpawnFromPool(movementScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position, transform.rotation, movementScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, movementScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movementScript.skillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        SkillManager primarySkill;
        ProjectileSkill primaryScript;

        public override void InitializePrimarySkill()
        {
            this.primaryScript = script.primarySkill as ProjectileSkill;
            objectPooler.AddToPool(primaryScript.skillPrefab, primaryScript.poolCount);
        }
        public override void TriggerPrimarySkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("primarySkill");
            audioManager.PlayAudio(AudioEnum.Character, primaryScript.skillVocalAudio);

            FXObject fxObj = objectPooler.SpawnFromPool(primaryScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position + transform.forward * 3, transform.rotation, primaryScript.lifetime);
        }
        public void HowlPrimaryTriggered()
        {
            audioManager.PlayAudio(AudioEnum.Action, primaryScript.skillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        SkillManager ultimateSkill;
        AOESkill ultimateScript;

        public override void InitializeUltimateSkill()
        {
            this.ultimateScript = script.ultimateSkill as AOESkill;
            objectPooler.AddToPool(ultimateScript.skillPrefab, ultimateScript.poolCount);
        }
        public override void TriggerUltimateSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateScript.skillVocalAudio);
        }
        public void HowlUltimateTriggered()
        {
            FXObject fxObj = objectPooler.SpawnFromPool(ultimateScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position, transform.rotation, ultimateScript.lifetime);
            audioManager.PlayAudio(AudioEnum.Action, ultimateScript.skillActionAudio);
        }
        #endregion
    }
}