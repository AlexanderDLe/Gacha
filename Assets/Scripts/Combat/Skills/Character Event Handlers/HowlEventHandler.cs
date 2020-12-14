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

        public override void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator)
        {
            this.audioManager = audioManager;
            this.objectPooler = objectPooler;
            this.raycaster = raycaster;
            this.animator = animator;
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
            movScript = script.movementSkill as MovementSkill;
            objectPooler.AddToPool(movScript.skillPrefab, movScript.poolCount);
        }
        public override void TriggerMovementSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("movementSkill");
        }
        public void HowlMovementStart()
        {
            EffectObject fxObj = objectPooler.SpawnFromPool(movScript.skillPrefab.name).GetComponent<EffectObject>();

            fxObj.Initialize(transform.position, transform.rotation, movScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, movScript.skillActionAudio);
            audioManager.PlayAudio(AudioEnum.Character, movScript.skillVocalAudio, probability);
        }
        #endregion

        #region Primary Skill
        SkillManager priSkill;
        AOESkill priScript;

        public override void InitializePrimarySkill()
        {
            this.priScript = script.primarySkill as AOESkill;
            objectPooler.AddToPool(priScript.skillPrefab, priScript.poolCount);
        }
        public override void TriggerPrimarySkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("primarySkill");
            audioManager.PlayAudio(AudioEnum.Character, priScript.skillVocalAudio);

            EffectObject fxObj = objectPooler.SpawnFromPool(priScript.skillPrefab.name).GetComponent<EffectObject>();
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
        SkillManager ultSkill;
        AOESkill ultScript;

        public override void InitializeUltimateSkill()
        {
            this.ultScript = script.ultimateSkill as AOESkill;
            objectPooler.AddToPool(ultScript.skillPrefab, ultScript.poolCount);
        }
        public override void TriggerUltimateSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultScript.skillVocalAudio);
        }
        public void HowlUltimateTriggered()
        {
            EffectObject fxObj = objectPooler.SpawnFromPool(ultScript.skillPrefab.name).GetComponent<EffectObject>();

            fxObj.Initialize(transform.position, transform.rotation, ultScript.lifetime);

            audioManager.PlayAudio(AudioEnum.Action, ultScript.skillActionAudio);
        }
        #endregion
    }
}