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

        public void Initialize(GameObject player_GO, GameObject character_GO, Animator animator, CharacterScriptableObject characterSO)
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

            movementSkill = InitializeSkill(player_GO, character_GO, animator,
                characterSO.movementSkill, "movementSkill");

            primarySkill = InitializeSkill(player_GO, character_GO, animator,
                characterSO.primarySkill, "primarySkill");

            ultimateSkill = InitializeSkill(player_GO, character_GO, animator,
                characterSO.ultimateSkill, "ultimateSkill");
        }

        private SkillManager InitializeSkill(GameObject player_GO, GameObject character_GO, Animator animator, SkillScriptableObject skillSO, string skillType)
        {
            SkillManager skill = character_GO.AddComponent<SkillManager>();
            skill.Initialize(player_GO, animator, skillType, skillSO);

            return skill;
        }
    }
}