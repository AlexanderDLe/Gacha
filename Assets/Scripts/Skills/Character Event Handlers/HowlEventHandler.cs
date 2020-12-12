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

        public AOEInvoker aoeCreator = null;

        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, AOEInvoker aoeCreator)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
            this.raycaster = raycaster;
            this.animator = animator;

            this.aoeCreator = aoeCreator;
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
        MovementSkill movementSkillScript;

        public override void InitializeMovementSkill()
        {
            movementSkillScript = script.movementSkill as MovementSkill;
            objectPooler.AddToPool(movementSkillScript.skillPrefab, 3);
        }
        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
        public void HowlMovementStart()
        {
            FXObject fxObj = objectPooler.SpawnFromPool(movementSkillScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position, transform.rotation, 3);

            audioManager.PlayAudio(AudioEnum.Action, movementSkillScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movementSkillScript.skillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        SkillManager primarySkill;
        ProjectileSkill primarySkillScript;

        public override void InitializePrimarySkill()
        {
            this.primarySkillScript = script.primarySkill as ProjectileSkill;
            objectPooler.AddToPool(primarySkillScript.skillPrefab, 3);
        }
        public override void TriggerPrimarySkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("primarySkill");
            audioManager.PlayAudio(AudioEnum.Character, primarySkillScript.skillVocalAudio);

            FXObject fxObj = objectPooler.SpawnFromPool(primarySkillScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position + transform.forward * 3, transform.rotation, 3);
        }
        public void HowlPrimaryTriggered()
        {
            audioManager.PlayAudio(AudioEnum.Action, primarySkillScript.skillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        SkillManager ultimateSkill;
        AOESkill ultimateSkillScript;

        public override void InitializeUltimateSkill()
        {
            this.ultimateSkillScript = script.ultimateSkill as AOESkill;
            objectPooler.AddToPool(ultimateSkillScript.skillPrefab, 3);
        }
        public override void TriggerUltimateSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillScript.skillVocalAudio);
        }
        public void HowlUltimateTriggered()
        {
            FXObject fxObj = objectPooler.SpawnFromPool(ultimateSkillScript.skillPrefab.name).GetComponent<FXObject>();

            fxObj.Initialize(transform.position, transform.rotation, 3);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillScript.skillActionAudio);
        }
        #endregion
    }
}