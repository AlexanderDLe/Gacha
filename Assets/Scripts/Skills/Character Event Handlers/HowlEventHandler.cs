using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Characters
{
    public class HowlEventHandler : AnimationEventHandler
    {
        public AudioManager audioManager;
        public BaseStats baseStats;
        public PlayableCharacter_SO script;
        public ObjectPooler objectPooler;

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
        }

        public override void InitializeSkills()
        {
            this.movementSkillVFX = script.movementSkill.skillVFX;
            this.movementSkillVocalAudio = script.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = script.movementSkill.skillActionAudio;

            this.primarySkillVFX = script.primarySkill.skillVFX;
            this.primarySkillVocalAudio = script.primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = script.primarySkill.skillActionAudio;

            this.ultimateSkillVFX = script.ultimateSkill.skillVFX;
            this.ultimateSkillVocalAudio = script.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = script.ultimateSkill.skillActionAudio;
        }

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;

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

        public void HowlPrimaryStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, primarySkillVocalAudio);
        }
        public void HowlPrimaryTrigger()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        public void HowlUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
        }
        public void HowlUltimateTrigger()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }
        #endregion
    }
}