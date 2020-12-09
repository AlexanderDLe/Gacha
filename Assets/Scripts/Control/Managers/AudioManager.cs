using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public enum AudioEnum
    {
        Default,
        Character,
        Action
    }

    public class AudioManager : MonoBehaviour
    {
        public AudioSource characterAudioSource = null;
        public AudioSource actionAudioSource = null;
        Dictionary<AudioEnum, AudioSource> audioDict = new Dictionary<AudioEnum, AudioSource>();

        public void SetAudioSources(AudioSource characterAudioSource, AudioSource actionAudioSource)
        {
            this.characterAudioSource = characterAudioSource;
            this.actionAudioSource = actionAudioSource;
            audioDict.Add(AudioEnum.Character, characterAudioSource);
            audioDict.Add(AudioEnum.Action, actionAudioSource);
        }

        public bool RandomlyDecideIfPlay(float probability)
        {
            if (Random.Range(0, 100) > probability * 100) return false;
            return true;
        }

        /*  Polymorphism Usage
            
            PlayAudio handles four different argument configurations.
            
            - Single clip with probability
            - Multi clip with probability
            - Single clip without probability
            - Multi clip without probability

            All of these overload functions take in an AudioEnum to
            decide which audio source to play from - whether it be
            the character audio source or the action audio source.
         */
        public void PlayAudio(AudioEnum audioEnum, AudioClip audioClip, float probability)
        {
            if (!RandomlyDecideIfPlay(probability)) return;
            audioDict[audioEnum].PlayOneShot(audioClip);
        }
        public void PlayAudio(AudioEnum audioEnum, AudioClip[] audioClips, float probability)
        {
            if (!RandomlyDecideIfPlay(probability)) return;
            int clipNumber = Random.Range(0, audioClips.Length);
            audioDict[audioEnum].PlayOneShot(audioClips[clipNumber]);
        }
        public void PlayAudio(AudioEnum audioEnum, AudioClip audioClip)
        {
            audioDict[audioEnum].PlayOneShot(audioClip);
        }
        public void PlayAudio(AudioEnum audioEnum, AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            audioDict[audioEnum].PlayOneShot(audioClips[clipNumber]);
        }
    }
}