using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        AudioSource audioSource = null;
        AudioSource characterAudioSource = null;
        AudioSource actionAudioSource = null;

        #region Initializations
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            characterAudioSource = gameObject.AddComponent<AudioSource>();
            actionAudioSource = gameObject.AddComponent<AudioSource>();
        }
        public void InitializeCharacterFX(CharacterScriptableObject character)
        {
            this.dashAudio = character.dashAudio;

            this.autoAttackVFX = character.autoAttackVFX;
            this.weakAttackAudio = character.weakAttackAudio;
            this.mediumAttackAudio = character.mediumAttackAudio;

            this.movementSkillVFX = character.movementSkill.skillVFX;
            this.movementSkillVocalAudio = character.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = character.movementSkill.skillActionAudio;

            this.primarySkillVFX = character.primarySkill.skillVFX;
            this.primarySkillVocalAudio = character.primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = character.primarySkill.skillActionAudio;

            this.ultimateSkillVFX = character.ultimateSkill.skillVFX;
            this.ultimateSkillVocalAudio = character.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = character.ultimateSkill.skillActionAudio;
        }
        #endregion

        #region Utility
        private bool RandomlyDecideIfPlay()
        {
            return Random.Range(0f, 10f) > 5f;
        }
        private void SelectAndPlayCharacterClip(AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            characterAudioSource.PlayOneShot(audioClips[clipNumber]);
        }
        private void SelectAndPlayActionClip(AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            actionAudioSource.PlayOneShot(audioClips[clipNumber]);
        }
        #endregion

        #region Dash
        private AudioClip[] dashAudio = null;
        private bool dashAudioJustPlayed = false;

        private void DashStart()
        {
            if (!RandomlyDecideIfPlay()) return;
            if (characterAudioSource.isPlaying || dashAudioJustPlayed) return;
            SelectAndPlayCharacterClip(dashAudio);
            StartCoroutine(TriggerDashAudio());
        }
        IEnumerator TriggerDashAudio()
        {
            dashAudioJustPlayed = true;
            yield return new WaitForSeconds(5);
            dashAudioJustPlayed = false;
        }
        #endregion

        #region Auto Attack
        // [SerializeField] AudioClip[] autoAttackClips = default;
        public GameObject[] autoAttackVFX = null;
        public AudioClip[] weakAttackAudio = null;
        public AudioClip[] mediumAttackAudio = null;

        public void Attack1()
        {

            SelectAndPlayCharacterClip(weakAttackAudio);
            Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
        }
        public void Attack2()
        {
            SelectAndPlayCharacterClip(mediumAttackAudio);
            Instantiate(autoAttackVFX[1], transform.position, transform.rotation);
        }
        #endregion

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;
        public void MovementSkillStart()
        {
            Instantiate(movementSkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(movementSkillActionAudio);
            if (RandomlyDecideIfPlay())
            {
                characterAudioSource.PlayOneShot(movementSkillVocalAudio);
            }
        }
        public void MovementSkillEnd() { }
        #endregion

        #region Primary Skill
        public GameObject primarySkillVFX = null;
        public AudioClip primarySkillActionAudio = null;
        public AudioClip primarySkillVocalAudio = null;
        public void PrimarySkillStart()
        {
            characterAudioSource.PlayOneShot(primarySkillVocalAudio);
        }
        public void PrimarySkillActivate()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        public GameObject ultimateSkillVFX = null;
        public AudioClip ultimateSkillActionAudio = null;
        public AudioClip ultimateSkillVocalAudio = null;
        public void UltimateSkillStart()
        {
            characterAudioSource.PlayOneShot(ultimateSkillVocalAudio);
        }
        public void UltimateSkillActivate()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(ultimateSkillActionAudio);
        }
        #endregion

        #region Footsteps
        [Header("Footsteps")]
        [SerializeField] AudioClip[] footstepClips = default;

        private void PlayRandomFootstepClip()
        {
            int num = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[num]);
        }
        private void Footstep()
        {
            PlayRandomFootstepClip();
        }
        #endregion
    }
}