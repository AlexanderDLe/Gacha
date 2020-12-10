using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using RPG.Skill;
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
            InitializeMetadata(char_SO);
            InitializeBaseStats(char_GO);
            InitializeEventHandler(animEventHandler);
            InitializeAttack(weapon);
            InitializeAllSkills(player_GO, char_GO, animator);
        }

        private void InitializeMetadata(PlayableCharacter_SO char_SO)
        {
            this.script = char_SO;
            this.avatar = char_SO.characterAvatar;
            this.name = char_SO.name;
            this.image = char_SO.image;
            this.animatorOverride = char_SO.animatorOverride;
        }

        private void InitializeBaseStats(GameObject char_GO)
        {
            this.baseStats = char_GO.AddComponent<BaseStats>();
            this.baseStats.Initialize(script);
        }

        private void InitializeEventHandler(AnimationEventHandler animEventHandler)
        {
            this.animEventHandler = animEventHandler;
            this.animEventHandler.Initialize(baseStats, script);
        }

        private void InitializeAttack(Weapon weapon)
        {
            this.weapon = weapon;
            this.numberOfAutoAttackHits = script.numberOfAutoAttackHits;
            this.autoAttackArray = GenerateAutoAttackArray(numberOfAutoAttackHits);
        }

        private void InitializeAllSkills(GameObject player_GO, GameObject char_GO, Animator animator)
        {
            this.movementSkillSprite = script.movementSkill.skillSprite;
            this.primarySkillSprite = script.primarySkill.skillSprite;
            this.ultimateSkillSprite = script.ultimateSkill.skillSprite;

            movementSkill = InitializeSkill(player_GO, char_GO, animator,
                script.movementSkill, "movementSkill", movementSkillSprite);

            primarySkill = InitializeSkill(player_GO, char_GO, animator,
                script.primarySkill, "primarySkill", primarySkillSprite);

            ultimateSkill = InitializeSkill(player_GO, char_GO, animator,
                script.ultimateSkill, "ultimateSkill", ultimateSkillSprite);
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