using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        #region Attributes
        public new string name;
        public float health;
        public BaseStats baseStats;
        #endregion

        #region Metadata
        [FoldoutGroup("Metadata")]
        public Sprite image;
        [FoldoutGroup("Metadata")]
        public Avatar avatar;
        [FoldoutGroup("Metadata")]
        public RuntimeAnimatorController animatorController;
        [FoldoutGroup("Metadata")]
        public AnimatorOverrideController animatorOverride;
        [FoldoutGroup("Metadata")]
        public Weapon weapon;
        #endregion

        #region Auto Attack
        [FoldoutGroup("Auto Attack")]
        public AttackTypeEnum attackType;
        [FoldoutGroup("Auto Attack")]
        public AutoAttack_SO attackScript;
        [FoldoutGroup("Auto Attack")]
        public int numberOfAutoAttackHits;
        [FoldoutGroup("Auto Attack")]
        public string[] autoAttackArray;
        [FoldoutGroup("Auto Attack")]
        public EffectPackage[] effectPackages;
        [FoldoutGroup("Auto Attack")]
        public Projectile_SO projectile_SO;
        [FoldoutGroup("Auto Attack")]
        public float[] autoAttackHitRadiuses;
        #endregion

        #region Skills
        [FoldoutGroup("MetaData")]
        public SkillEventHandler skillEventHandler;
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

        public void Initialize(GameObject player_GO, GameObject char_GO, Animator animator, PlayableCharacter_SO char_SO, Weapon weapon, SkillEventHandler animEventHandler)
        {
            InitializeMetadata(char_SO);
            InitializeBaseStats(char_GO);
            InitializeAttack(weapon);
            InitializeAllSkills(player_GO, char_GO, animator);
            InitializeSkillEventHandler(animEventHandler);
        }

        private void InitializeMetadata(PlayableCharacter_SO char_SO)
        {
            this.script = char_SO;
            this.avatar = char_SO.characterAvatar;
            this.name = char_SO.name;
            this.image = char_SO.image;
            this.animatorController = char_SO.animatorController;
        }

        private void InitializeBaseStats(GameObject char_GO)
        {
            this.baseStats = char_GO.AddComponent<BaseStats>();
            this.baseStats.Initialize(script);
        }

        private void InitializeAttack(Weapon weapon)
        {
            this.weapon = weapon;
            this.attackType = script.attackType;
            this.attackScript = script.autoAttack_SO;

            this.effectPackages = attackScript.effectPackages;
            this.numberOfAutoAttackHits = attackScript.numberOfAutoAttackHits;
            this.autoAttackArray = GenerateAutoAttackArray(numberOfAutoAttackHits);

            if (this.attackType == AttackTypeEnum.Projectile)
            {
                ProjectileAttack_SO proj_SO = attackScript as ProjectileAttack_SO;
                this.projectile_SO = proj_SO.projectile;
            }
            if (this.attackType == AttackTypeEnum.Melee)
            {
                MeleeAttack_SO melee_SO = attackScript as MeleeAttack_SO;
                this.autoAttackHitRadiuses = melee_SO.autoAttackHitRadiuses;
            }
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

        private void InitializeSkillEventHandler(SkillEventHandler animEventHandler)
        {
            this.skillEventHandler = animEventHandler;
            this.skillEventHandler.Initialize(baseStats, script);
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

        // Skill Triggers
        public void TriggerMovementSkill()
        {
            movementSkill.TriggerSkill();
            skillEventHandler.TriggerMovementSkill();
        }
        public void TriggerPrimarySkill()
        {
            primarySkill.TriggerSkill();
            skillEventHandler.TriggerPrimarySkill();
        }
        public void TriggerUltimateSkill()
        {
            ultimateSkill.TriggerSkill();
            skillEventHandler.TriggerUltimateSkill();
        }
    }
}