using System.Collections;
using RPG.Core;
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
            string skillType, SkillScriptableObject skillScriptableObject, Sprite skillImage)
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
            skill.Initialize(player_GO, skillType);
        }

        [Header("Skill Cooldown")]
        public SkillScriptableObject skill = null;

        public string skillName;
        public bool isUsingSkill = false;
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

        public bool GetIsSkillInCooldown()
        {
            return skillInCooldown;
        }
        public void SetIsUsingSkill(bool value)
        {
            isUsingSkill = value;
        }
        public bool GetIsUsingSkill()
        {
            return isUsingSkill;
        }
        public bool IsInSkillAnimation()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(skillType))
            {
                return true;
            }
            return false;
        }

        //  Skill Aiming
        public bool SkillRequiresAim()
        {
            return requiresSkillShot || requiresRangeShot;
        }
        public bool GetAimingEnabled()
        {
            return isAimingSkill;
        }
        public void SetAimingEnabled(bool value)
        {
            isAimingSkill = value;
        }
        public void ActivateSkillAim()
        {
            SetAimingEnabled(true);
            string skillType = requiresSkillShot ? SKILLSHOT : RANGESHOT;
        }
        //  Skill Triggers
        public void TriggerSkill()
        {
            skill.TriggerSkill();
            SetIsUsingSkill(true);
            StartCoroutine(SkillCountdown());
        }
        IEnumerator SkillCountdown()
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