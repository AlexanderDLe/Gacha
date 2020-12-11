using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
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
        }

        public override void InitializeMovementSkill()
        {
            this.movementSkillVFX = script.movementSkill.skillPrefab;
            this.movementSkillVocalAudio = script.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = script.movementSkill.skillActionAudio;

            this.primarySkillVFX = script.primarySkill.skillPrefab;
            this.primarySkillVocalAudio = script.primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = script.primarySkill.skillActionAudio;

            this.ultimateSkillVFX = script.ultimateSkill.skillPrefab;
            this.ultimateSkillVocalAudio = script.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = script.ultimateSkill.skillActionAudio;
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
        public void HowlMovementStart()
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

        public override void TriggerPrimarySkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("primarySkill");
        }
        public void HowlPrimaryStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, primarySkillVocalAudio);
        }
        public void HowlPrimaryTriggered()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;

        public override void TriggerUltimateSkill()
        {
            raycaster.RotateObjectTowardsMousePosition(gameObject);
            animator.SetTrigger("ultimateSkill");
        }
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
        }
        public void HowlUltimateTriggered()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }

        #endregion
    }
}