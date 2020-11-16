using UnityEngine;

namespace RPG.Core
{
    public class FXManager : MonoBehaviour
    {
        AudioSource audioSource = null;

        #region Initializations
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        private void IntializeCharacterFX(CharacterScriptableObject character)
        {
            autoAttackVFX = character.autoAttackFX;
        }
        #endregion

        #region Footsteps
        [Header("Misc")]
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

        #region Auto Attack FX
        [SerializeField] AudioClip[] autoAttackClips = default;
        int numberOfAutoAttackHits = 0;
        public GameObject[] autoAttackVFX = null;
        #endregion
    }
}