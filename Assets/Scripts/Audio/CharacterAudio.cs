using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] dashClips;
    [SerializeField] AudioClip[] weakAttackClips;
    [SerializeField] AudioClip[] strongAttackClips;
    [SerializeField] AudioClip[] ultimateSkillClips;
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
        audioSource.PlayOneShot(GenerateRandomNum(dashClips));
    }
    private void Attack1()
    {
        audioSource.PlayOneShot(GenerateRandomNum(weakAttackClips));
    }
    private void Attack2()
    {
        audioSource.PlayOneShot(GenerateRandomNum(strongAttackClips));
    }
}
