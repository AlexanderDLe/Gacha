using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class ActionManager : MonoBehaviour
    {
        public GameObject environment = null;
        ObjectPooler objectPooler = null;
        RaycastMousePosition raycaster = null;
        BaseStats baseStats = null;
        AudioManager audioPlayer = null;

        #region Initializations
        public void LinkReferences(AudioManager audioPlayer, RaycastMousePosition raycaster, ObjectPooler objectPooler)
        {
            this.audioPlayer = audioPlayer;
            this.raycaster = raycaster;
            this.objectPooler = objectPooler;
        }

        public void Initialize(CharacterManager character, BaseStats baseStats)
        {
            Weapon weapon = character.weapon;
            this.baseStats = baseStats;

            InitializeFX(character.script);
        }

        private void InitializeFX(PlayableCharacter_SO char_SO)
        {
            this.movementSkillVFX = char_SO.movementSkill.skillVFX;
            this.movementSkillVocalAudio = char_SO.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = char_SO.movementSkill.skillActionAudio;

            this.primarySkillVFX = char_SO.primarySkill.skillVFX;
            this.primarySkillVocalAudio = char_SO.primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = char_SO.primarySkill.skillActionAudio;

            this.ultimateSkillVFX = char_SO.ultimateSkill.skillVFX;
            this.ultimateSkillVocalAudio = char_SO.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = char_SO.ultimateSkill.skillActionAudio;
        }
        #endregion


        #region Swap Character
        [FoldoutGroup("Character Swap")]
        [SerializeField] GameObject swapVisualFX = null;
        [FoldoutGroup("Character Swap")]
        [SerializeField] AudioClip swapAudioFX = null;

        public void ActivateSwapFX()
        {
            GameObject swapVFX = Instantiate(swapVisualFX, transform);
            swapVFX.transform.SetParent(environment.transform);
            audioPlayer.PlayCharacterClip(swapAudioFX);
        }
        #endregion

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;

        public void MovementSkillStart()
        {
            Instantiate(movementSkillVFX, transform.position, transform.rotation);
            audioPlayer.PlayActionClip(movementSkillActionAudio);
            if (audioPlayer.RandomlyDecideIfPlay())
            {
                audioPlayer.PlayCharacterClip(movementSkillVocalAudio);
            }
        }
        public void MovementSkillActivate()
        {
            // Instantiate(movementSkillVFX, transform.position, transform.rotation);
            // audioPlayer.PlayActionClip(movementSkillActionAudio);
            // if (audioPlayer.RandomlyDecideIfPlay())
            // {
            //     audioPlayer.PlayCharacterClip(movementSkillVocalAudio);
            // }
        }
        public void MovementSkillEnd() { }
        #endregion

        #region Primary Skill
        [FoldoutGroup("Primary Skill FX")]
        GameObject primarySkillVFX = null;
        [FoldoutGroup("Primary Skill FX")]
        AudioClip primarySkillActionAudio = null;
        [FoldoutGroup("Primary Skill FX")]
        AudioClip primarySkillVocalAudio = null;
        public void PrimarySkillStart()
        {
            audioPlayer.PlayCharacterClip(primarySkillVocalAudio);
        }
        public void PrimarySkillActivate()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            audioPlayer.PlayActionClip(primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        public void UltimateSkillStart()
        {
            audioPlayer.PlayCharacterClip(ultimateSkillVocalAudio);
        }
        public void UltimateSkillActivate()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            audioPlayer.PlayActionClip(ultimateSkillActionAudio);
        }
        #endregion

        #region Footsteps
        [Header("Footsteps")]
        [SerializeField] AudioClip[] footstepClips = default;

        private void PlayRandomFootstepClip()
        {
            int num = Random.Range(0, footstepClips.Length);
            audioPlayer.PlayCharacterClip(footstepClips[num]);
        }
        private void Footstep()
        {
            PlayRandomFootstepClip();
        }
        #endregion
    }
}