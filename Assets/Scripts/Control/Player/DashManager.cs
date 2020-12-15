using System;
using System.Collections;
using RPG.Characters;
using UnityEngine;

namespace RPG.Control
{
    public class DashManager : MonoBehaviour
    {
        private void Start()
        {
            currentDashCharges = maxDashCharges;
        }
        public void LinkReferences(AudioManager audioPlayer)
        {
            this.audioManager = audioPlayer;
        }
        public void Initialize(CharacterManager character)
        {
            this.dashAudio = character.script.dashAudio;
        }

        #region Dash Mechanics
        public int maxDashCharges = 3;
        public int currentDashCharges = 3;
        public float dashRegenRate = 3f;
        public float dashSpeed = 20f;
        public bool isDashing = false;

        public bool GetCanDash() => currentDashCharges > 0;

        public bool IsDashing() => isDashing;

        public void SetIsDashing(bool value) => isDashing = value;

        public float GetDashSpeed() => dashSpeed;

        public void TriggerDash()
        {
            isDashing = true;
            StartCoroutine(RegenDashCharge());
        }

        public event Action OnDashUpdate;
        IEnumerator RegenDashCharge()
        {
            currentDashCharges--;
            OnDashUpdate();
            yield return new WaitForSeconds(dashRegenRate);
            currentDashCharges++;
            OnDashUpdate();
        }
        #endregion

        #region Dash Audio
        AudioManager audioManager = null;
        private AudioClip[] dashAudio = null;
        private bool dashAudioJustPlayed = false;

        private void DashStart()
        {
            if (!audioManager.RandomlyDecideIfPlay(.5f)) return;
            if (audioManager.characterAudioSource.isPlaying || dashAudioJustPlayed) return;
            audioManager.PlayAudio(AudioEnum.Character, dashAudio);
            StartCoroutine(TriggerDashAudio());
        }

        IEnumerator TriggerDashAudio()
        {
            dashAudioJustPlayed = true;
            yield return new WaitForSeconds(5);
            dashAudioJustPlayed = false;
        }
        #endregion
    }
}