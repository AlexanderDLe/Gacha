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
            this.primarySkillFX = character.primarySkillFX;
            this.primarySkillVocalAudio = character.primarySkillVocalAudio;
            this.primarySkillActionAudio = character.primarySkillActionAudio;
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

        public void Attack1()
        {
            Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
        }
        public void Attack2()
        {
            Instantiate(autoAttackVFX[1], transform.position, transform.rotation);
        }
        #endregion

        #region Primary Skill
        public GameObject primarySkillFX = null;
        public AudioClip primarySkillActionAudio = null;
        public AudioClip primarySkillVocalAudio = null;
        public void PrimarySkillStart()
        {
            characterAudioSource.PlayOneShot(primarySkillVocalAudio);
        }
        public void PrimarySkillActivate()
        {
            Instantiate(primarySkillFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(primarySkillActionAudio);
        }
        #endregion

        #region Primary Skill Shot

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