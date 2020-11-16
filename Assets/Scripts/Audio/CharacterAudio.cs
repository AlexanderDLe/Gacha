using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    // [SerializeField] AudioClip[] dashClips = default;
    // [SerializeField] AudioClip[] weakAttackClips = default;
    // [SerializeField] AudioClip[] strongAttackClips = default;
    // [SerializeField] AudioClip[] ultimateSkillClips = default;
    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GenerateRandomNum(AudioClip[] clips)
    {
        int randomNum = Random.Range(0, clips.Length);
        return clips[randomNum];
    }

    private void StartDash()
    {
        // audioSource.PlayOneShot(GenerateRandomNum(dashClips));
    }
    private void Attack1()
    {
        // audioSource.PlayOneShot(GenerateRandomNum(weakAttackClips));
    }
    private void Attack2()
    {
        // audioSource.PlayOneShot(GenerateRandomNum(strongAttackClips));
    }
}
