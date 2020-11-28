using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        #region Attributes
        public new string name;
        public float health;
        public int numberOfAutoAttackHits;
        public string[] autoAttackArray;
        public BaseStats baseStats;
        public Weapon weapon;
        #endregion

        #region Metadata
        [FoldoutGroup("Metadata")]
        public Sprite image;
        [FoldoutGroup("Metadata")]
        public Avatar avatar;
        [FoldoutGroup("Metadata")]
        public AnimatorOverrideController animatorOverride;
        #endregion

        #region Skills
        [FoldoutGroup("Skills")]
        public SkillManager movementSkill;
        [FoldoutGroup("Skills")]
        public SkillManager primarySkill;
        [FoldoutGroup("Skills")]
        public SkillManager ultimateSkill;

        [FoldoutGroup("Skills")]
        public Sprite movementSkillSprite;
        [FoldoutGroup("Skills")]
        public Sprite primarySkillSprite;
        [FoldoutGroup("Skills")]
        public Sprite ultimateSkillSprite;
        #endregion

        public PlayableCharacter_SO script = null;

        public void Initialize(GameObject player_GO, GameObject character_GO, Animator animator, PlayableCharacter_SO character_SO, Weapon weapon)
        {
            this.baseStats = character_GO.AddComponent<BaseStats>();
            this.baseStats.Initialize(character_SO);

            this.weapon = weapon;

            this.script = character_SO;
            this.avatar = character_SO.characterAvatar;
            this.name = character_SO.name;
            this.image = character_SO.image;
            this.numberOfAutoAttackHits = character_SO.numberOfAutoAttackHits;
            this.autoAttackArray = GenerateAutoAttackArray(numberOfAutoAttackHits);

            this.animatorOverride = character_SO.animatorOverride;

            this.movementSkillSprite = character_SO.movementSkill.skillSprite;
            this.primarySkillSprite = character_SO.primarySkill.skillSprite;
            this.ultimateSkillSprite = character_SO.ultimateSkill.skillSprite;

            movementSkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.movementSkill, "movementSkill", movementSkillSprite);

            primarySkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.primarySkill, "primarySkill", primarySkillSprite);

            ultimateSkill = InitializeSkill(player_GO, character_GO, animator,
                character_SO.ultimateSkill, "ultimateSkill", ultimateSkillSprite);
        }

        private SkillManager InitializeSkill(GameObject player_GO, GameObject character_GO, Animator animator, SkillScriptableObject skillSO, string skillType, Sprite skillImage)
        {
            SkillManager skill = character_GO.AddComponent<SkillManager>();
            skill.Initialize(player_GO, animator, skillType, skillSO, skillImage);

            return skill;
        }

        public void CancelSkillAiming()
        {
            movementSkill.SetAimingEnabled(false);
            primarySkill.SetAimingEnabled(false);
            ultimateSkill.SetAimingEnabled(false);
        }

        private string[] GenerateAutoAttackArray(int numOfAutoAttackHits)
        {
            string[] autoAttackTempArray = new string[numOfAutoAttackHits];
            for (int i = 0; i < numOfAutoAttackHits; i++)
            {
                autoAttackTempArray[i] = ("attack" + (i + 1).ToString());
            }
            return autoAttackTempArray;
        }
    }
}