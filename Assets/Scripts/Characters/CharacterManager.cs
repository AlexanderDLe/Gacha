using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public new string name;
        public float health;
        public Sprite image;
        public int numberOfAutoAttackHits;
        public GameObject prefab;
        public Avatar avatar;
        public AnimatorOverrideController animatorOverride;

        public SkillManager movementSkill;
        public SkillManager primarySkill;
        public SkillManager ultimateSkill;

        public Sprite movementSkillSprite;
        public Sprite primarySkillSprite;
        public Sprite ultimateSkillSprite;

        public CharacterScriptableObject charSO = null;

        public void Initialize(GameObject player_GO, GameObject character_GO, Animator animator, CharacterScriptableObject character_SO)
        {
            this.prefab = character_SO.prefab;
            this.charSO = character_SO;
            this.avatar = character_SO.characterAvatar;
            this.name = character_SO.name;
            this.health = character_SO.health;
            this.image = character_SO.image;
            this.numberOfAutoAttackHits = character_SO.numberOfAutoAttackHits;
            this.animatorOverride = character_SO.animatorOverride;

            this.movementSkillSprite = character_SO.movementSkill.skillSprite;
            this.primarySkillSprite = character_SO.primarySkill.skillSprite;
            this.ultimateSkillSprite = character_SO.ultimateSkill.skillSprite;

            movementSkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.movementSkill, "movementSkill");

            primarySkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.primarySkill, "primarySkill");

            ultimateSkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.ultimateSkill, "ultimateSkill");
        }

        private SkillManager InitializeSkill(GameObject player_GO, GameObject character_GO, Animator animator, SkillScriptableObject skillSO, string skillType)
        {
            SkillManager skill = character_GO.AddComponent<SkillManager>();
            skill.Initialize(player_GO, animator, skillType, skillSO);

            return skill;
        }
    }
}