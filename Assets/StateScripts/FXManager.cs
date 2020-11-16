using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class FXManager : MonoBehaviour
    {
        AudioSource audioSource = null;
        AudioSource characterAudioSource = null;

        #region Initializations
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            characterAudioSource = gameObject.AddComponent<AudioSource>();
        }
        public void InitializeCharacterFX(CharacterScriptableObject character)
        {
            this.autoAttackVFX = character.autoAttackVFX;
            this.dashAudio = character.dashAudio;
        }
        #endregion

        #region Methods
        private bool RandomlyDecideIfPlay()
        {
            return Random.Range(0f, 10f) > 5f;
        }
        private void SelectAndPlayClip(AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            audioSource.PlayOneShot(audioClips[clipNumber]);
        }
        #endregion

        #region Dash
        private AudioClip[] dashAudio = null;
        private bool dashAudioJustPlayed = false;

        private void StartDash()
        {
            if (!RandomlyDecideIfPlay()) return;
            if (characterAudioSource.isPlaying || dashAudioJustPlayed) return;
            SelectAndPlayClip(dashAudio);
            StartCoroutine(TriggerDashAudio());
        }
        IEnumerator TriggerDashAudio()
        {
            dashAudioJustPlayed = true;
            yield return new WaitForSeconds(5);
            dashAudioJustPlayed = false;
        }
        #endregion

        #region Auto Attack FX
        [SerializeField] AudioClip[] autoAttackClips = default;
        // int numberOfAutoAttackHits = 0;
        public GameObject[] autoAttackVFX = null;

        public void Attack1()
        {
            // Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
        }
        public void Attack2()
        {
            // Instantiate(autoAttackVFX[1], transform.position, transform.rotation);
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