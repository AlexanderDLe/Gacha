﻿using System;
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
        public void LinkReferences(AudioPlayer audioPlayer)
        {
            this.audioPlayer = audioPlayer;
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
        AudioPlayer audioPlayer = null;
        private AudioClip[] dashAudio = null;
        private bool dashAudioJustPlayed = false;

        private void DashStart()
        {
            if (!audioPlayer.RandomlyDecideIfPlay()) return;
            if (audioPlayer.characterAudioSource.isPlaying || dashAudioJustPlayed) return;
            audioPlayer.SelectAndPlayCharacterClip(dashAudio);
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