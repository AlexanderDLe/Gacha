using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public string characterName;
        public float characterHealth;
        public Sprite characterImage;
        public int numberOfAutoAttackHits;
        public GameObject characterPrefab;
        public AnimatorOverrideController animatorOverride;

        public SkillManager movementSkill;
        public SkillManager primarySkill;
        public SkillManager ultimateSkill;

        public Sprite movementSkillSprite;
        public Sprite primarySkillSprite;
        public Sprite ultimateSkillSprite;

        public CharacterScriptableObject charScript = null;

        public void Initialize(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, CharacterScriptableObject characterSO)
        {
            this.characterPrefab = characterSO.characterPrefab;
            this.charScript = characterSO;
            this.characterName = characterSO.characterName;
            this.characterHealth = characterSO.characterHealth;
            this.characterImage = characterSO.characterImage;
            this.numberOfAutoAttackHits = characterSO.numberOfAutoAttackHits;
            this.animatorOverride = characterSO.animatorOverride;

            InitializeMovementSkill(gameObject, animator, raycaster, characterSO);
            InitializePrimarySkill(gameObject, animator, raycaster, characterSO);
            InitializeUltimateSkill(gameObject, animator, raycaster, characterSO);

            movementSkillSprite = characterSO.movementSkill.skillSprite;
            primarySkillSprite = characterSO.primarySkill.skillSprite;
            ultimateSkillSprite = characterSO.ultimateSkill.skillSprite;
        }

        private void InitializeMovementSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, CharacterScriptableObject characterSO)
        {
            movementSkill = gameObject.AddComponent<SkillManager>();
            movementSkill.Initialize(gameObject, animator, raycaster, "movementSkill", characterSO.movementSkill);
        }

        private void InitializePrimarySkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, CharacterScriptableObject characterSO)
        {
            primarySkill = gameObject.AddComponent<SkillManager>();
            primarySkill.Initialize(gameObject, animator, raycaster, "primarySkill", characterSO.primarySkill);
        }

        private void InitializeUltimateSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, CharacterScriptableObject characterSO)
        {
            ultimateSkill = gameObject.AddComponent<SkillManager>();
            ultimateSkill.Initialize(gameObject, animator, raycaster, "ultimateSkill", characterSO.ultimateSkill);
        }
    }
}