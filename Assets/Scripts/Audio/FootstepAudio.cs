using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayRandomFootstepClip()
    {
        int num = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[num]);
    }

    // Unity Triggered Events
    private void Footstep()
    {
        PlayRandomFootstepClip();
    }
}
