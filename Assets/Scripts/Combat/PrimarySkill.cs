using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class PrimarySkill : MonoBehaviour
    {
        private Animator animator = null;
        RaycastMousePosition raycaster = null;
        SkillShot skillshotter = null;
        Vector3 mousePosition;

        [SerializeField] GameObject primarySkillFX = null;

        [Header("Primary Skill Cooldown")]
        [SerializeField] bool skillInCooldown = false;
        [Tooltip("The time it takes to reset primary skill use.")]
        [SerializeField] float cooldownResetTime = 2f;
        [Tooltip("The current time in the cooldown cycle.")]
        [SerializeField] float cooldownTimer = 0f;

        [Header("Use Visual Aim?")]
        [SerializeField] bool requireSkillshot = false;
        [SerializeField] bool requireCircleAim = false;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            raycaster = GetComponent<RaycastMousePosition>();
            skillshotter = GetComponent<SkillShot>();
        }

        private void Update()
        {
            UpdatePrimarySkillCycle();

            if (skillshotter.isUsingSkillshot) skillshotter.AimWithSkillshot();
            if (skillshotter.isUsingCircleAim) skillshotter.AimWithCircleAim();
        }

        public void TriggerPrimarySkill(Vector3 target)
        {
            if (requireSkillshot && !skillshotter.isUsingSkillshot && !skillInCooldown)
            {
                skillshotter.ActivateSkillShotAim();
                return;
            }

            if (requireCircleAim && !skillshotter.isUsingCircleAim && !skillInCooldown)
            {
                skillshotter.ActivateCircleAim();
                return;
            }

            if (!skillInCooldown)
            {
                transform.LookAt(target);
                animator.SetTrigger("primarySkill");
                print("Function triggered bool to true.");
                skillInCooldown = true;
            }
        }

        public void CancelPrimarySkill()
        {
            skillshotter.DeactivateAim();
            animator.SetTrigger("resetAttack");
        }

        public bool IsAimingPrimarySkill()
        {
            return skillshotter.isUsingSkillshot || skillshotter.isUsingCircleAim;
        }
        public bool IsInPrimarySkillAnimation()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("primarySkill");
        }

        private void UpdatePrimarySkillCycle()
        {
            if (skillInCooldown)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= cooldownResetTime)
                {
                    cooldownTimer = 0f;
                    skillInCooldown = false;
                }
            }
        }

        // Animator Triggered Events
        private void StartPrimarySkill()
        {
        }

        private void PrimarySkillAttack()
        {
            Instantiate(primarySkillFX, transform.position, transform.rotation);
            skillshotter.DeactivateAim();
        }
    }
}