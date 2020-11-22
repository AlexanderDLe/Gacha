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

            this.movementSkillSprite = characterSO.movementSkill.skillSprite;
            this.primarySkillSprite = characterSO.primarySkill.skillSprite;
            this.ultimateSkillSprite = characterSO.ultimateSkill.skillSprite;

            movementSkill = InitializeSkill(gameObject, animator, raycaster,
                characterSO.movementSkill, "movementSkill");

            primarySkill = InitializeSkill(gameObject, animator, raycaster,
                characterSO.primarySkill, "primarySkill");

            ultimateSkill = InitializeSkill(gameObject, animator, raycaster,
                characterSO.ultimateSkill, "ultimateSkill");
        }

        private SkillManager InitializeSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, SkillScriptableObject skillSO, string skillType)
        {
            SkillManager skill = gameObject.AddComponent<SkillManager>();
            skill.Initialize(gameObject, animator, raycaster, skillType, skillSO);

            return skill;
        }
    }
}