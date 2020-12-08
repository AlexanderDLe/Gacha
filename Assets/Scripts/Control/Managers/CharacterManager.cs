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
        [FoldoutGroup("MetaData")]
        public AnimationEventHandler animEventHandler;
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

        public void Initialize(GameObject player_GO, GameObject char_GO, Animator animator, PlayableCharacter_SO char_SO, Weapon weapon, AnimationEventHandler animEventHandler)
        {
            this.baseStats = char_GO.AddComponent<BaseStats>();
            this.baseStats.Initialize(char_SO);
            this.animEventHandler = animEventHandler;

            this.weapon = weapon;

            this.script = char_SO;
            this.avatar = char_SO.characterAvatar;
            this.name = char_SO.name;
            this.image = char_SO.image;
            this.numberOfAutoAttackHits = char_SO.numberOfAutoAttackHits;
            this.autoAttackArray = GenerateAutoAttackArray(numberOfAutoAttackHits);

            this.animatorOverride = char_SO.animatorOverride;

            this.movementSkillSprite = char_SO.movementSkill.skillSprite;
            this.primarySkillSprite = char_SO.primarySkill.skillSprite;
            this.ultimateSkillSprite = char_SO.ultimateSkill.skillSprite;

            movementSkill = InitializeSkill(player_GO, char_GO, animator,
                char_SO.movementSkill, "movementSkill", movementSkillSprite);

            primarySkill = InitializeSkill(player_GO, char_GO, animator,
                char_SO.primarySkill, "primarySkill", primarySkillSprite);

            ultimateSkill = InitializeSkill(player_GO, char_GO, animator,
                char_SO.ultimateSkill, "ultimateSkill", ultimateSkillSprite);
        }

        private SkillManager InitializeSkill(GameObject player_GO, GameObject character_GO, Animator animator, Skill_SO skillSO, string skillType, Sprite skillImage)
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