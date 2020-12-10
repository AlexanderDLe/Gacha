using System.Collections;
using RPG.Skill;
using UnityEngine;

namespace RPG.Control
{
    public class SkillManager : MonoBehaviour
    {
        Animator animator = null;
        string skillType;
        string SKILLSHOT = "SKILLSHOT";
        string RANGESHOT = "RANGESHOT";

        public void Initialize(GameObject player_GO, Animator animator,
            string skillType, Skill_SO skillScriptableObject, Sprite skillImage)
        {
            this.skill = skillScriptableObject;
            this.skillName = skill.skillName;
            this.animator = animator;
            this.skillType = skillType;
            this.skillImage = skillImage;

            this.cooldownResetTime = skill.baseCooldownTime;
            this.requiresSkillShot = skill.requiresSkillShot;
            this.requiresRangeShot = skill.requiresRangeShot;
            this.isAimingSkill = false;

            if (skill.requiresSkillShot)
            {
                this.skillShotImage = skill.skillShotImage;
            }
            if (skill.requiresRangeShot)
            {
                this.rangeImage = skill.skillRangeImage;
                this.reticleImage = skill.skillReticleImage;
            }
        }

        public Skill_SO skill = null;
        public bool isUsingSkill = false;
        public string skillName;

        [Header("Skill Cooldown")]
        private bool skillInCooldown = false;
        public float cooldownResetTime = 3f;
        public float skillCountdownTimer = 0f;

        [Header("Skill Aiming")]
        public bool requiresSkillShot = false;
        public bool requiresRangeShot = false;
        public bool isAimingSkill = false;

        public Sprite skillImage = null;
        public Sprite skillShotImage = null;
        public Sprite rangeImage = null;
        public Sprite reticleImage = null;

        public bool IsSkillInCooldown() => skillInCooldown;

        public void SetIsUsingSkill(bool value) => isUsingSkill = value;

        public bool IsUsingSkill() => isUsingSkill;

        public bool SkillRequiresAim() => requiresSkillShot || requiresRangeShot;

        public bool IsAimingEnabled() => isAimingSkill;

        public void SetAimingEnabled(bool value) => isAimingSkill = value;

        public bool IsInSkillAnimation()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(skillType))
            {
                return true;
            }
            return false;
        }

        public void ActivateSkillAim()
        {
            SetAimingEnabled(true);
            string skillType = requiresSkillShot ? SKILLSHOT : RANGESHOT;
        }

        public void TriggerSkill()
        {
            SetIsUsingSkill(true);
            StartCoroutine(TriggerSkillCooldown());
        }

        IEnumerator TriggerSkillCooldown()
        {
            skillInCooldown = true;
            skillCountdownTimer = cooldownResetTime;
            while (skillCountdownTimer > 0)
            {
                skillCountdownTimer -= Time.deltaTime;
                yield return null;
            }
            skillInCooldown = false;
        }

        public void SkillActivate()
        {
            SetIsUsingSkill(false);
        }
    }
}