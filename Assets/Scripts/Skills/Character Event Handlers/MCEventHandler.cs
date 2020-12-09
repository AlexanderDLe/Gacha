using System;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Characters
{
    public class MCEventHandler : AnimationEventHandler
    {
        public AudioManager audioManager;
        public BaseStats baseStats;
        public PlayableCharacter_SO script;

        private void Awake()
        {
            audioManager = GetComponent<AudioManager>();
        }
        public override void Initialize(BaseStats baseStats, PlayableCharacter_SO script)
        {
            this.baseStats = baseStats;
            this.script = script;
            InitializeFX();
        }

        public override void InitializeFX()
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

        public void MCPrimaryStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, primarySkillVocalAudio);
        }
        public void MCPrimaryTrigger()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        public void MCUltimateStart()
        {
            audioManager.PlayAudio(AudioEnum.Character, ultimateSkillVocalAudio);
        }
        public void MCUltimateTrigger()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioManager.PlayAudio(AudioEnum.Action, ultimateSkillActionAudio);
        }
        #endregion
    }
}