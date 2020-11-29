using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource characterAudioSource = null;
    public AudioSource actionAudioSource = null;

    public void SetAudioSources(AudioSource characterAudioSource, AudioSource actionAudioSource)
    {
        this.characterAudioSource = characterAudioSource;
        this.actionAudioSource = actionAudioSource;
    }

    public bool RandomlyDecideIfPlay()
    {
        return Random.Range(0f, 10f) > 5f;
    }
    public void SelectAndPlayCharacterClip(AudioClip[] audioClips)
    {
        int clipNumber = Random.Range(0, audioClips.Length);
        characterAudioSource.PlayOneShot(audioClips[clipNumber]);
    }
    public void SelectAndPlayActionClip(AudioClip[] audioClips)
    {
        int clipNumber = Random.Range(0, audioClips.Length);
        actionAudioSource.PlayOneShot(audioClips[clipNumber]);
    }
    public void PlayCharacterClip(AudioClip audioClip)
    {
        characterAudioSource.PlayOneShot(audioClip);
    }
    public void PlayActionClip(AudioClip audioClip)
    {
        actionAudioSource.PlayOneShot(audioClip);
    }
}
