using System;
using System.Collections;
using UnityEngine;
using System.Drawing;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using RPG.Control;

namespace RPG.Core
{
    public class StateManager : MonoBehaviour
    {
        #region Start
        Animator animator = null;
        ActionManager actionManager = null;
        RaycastMousePosition raycaster = null;
        Vector3 mousePosition = Vector3.zero;
        public CharacterScriptableObject currentCharacter = null;

        public SkillManager ultimateSkill = null;
        public SkillManager primarySkill = null;
        public SkillManager movementSkill = null;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            raycaster = GetComponent<RaycastMousePosition>();

            ultimateSkill = gameObject.AddComponent<SkillManager>();
            primarySkill = gameObject.AddComponent<SkillManager>();
            movementSkill = gameObject.AddComponent<SkillManager>();
        }
        private void Start()
        {
            IntializeCharacter(currentCharacter);
            currentDashCharges = maxDashCharges;
        }
        private void Update()
        {
            if (skillshotAimingActive) AimWithSkillshot();
            if (rangeshotAimingActive) AimWithRangeshot();
        }
        #endregion

        #region Permissions
        public bool CanMove()
        {
            if (isDashing) return false;
            if (isInAutoAttackState || IsInAutoAttackAnimation()) return false;
            if (IsUsingAnySkill() || IsInAnySkillAnimation()) return false;
            return true;
        }
        public bool CanDash()
        {
            if (isDashing) return false;
            if (currentDashCharges == 0) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanAutoAttack()
        {
            if (isDashing) return false;
            if (isInAutoAttackState) return false;
            if (IsUsingAnySkill()) return false;
            if (IsInAutoAttackAnimation()) return false;
            return true;
        }
        public bool CanUseMovementSkill()
        {
            if (isDashing) return false;
            if (GetIsSkillInCooldown(movementSkill)) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUsePrimarySkill()
        {
            if (isDashing) return false;
            if (GetIsSkillInCooldown(primarySkill)) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUseUltimateSkill()
        {
            if (isDashing) return false;
            if (GetIsSkillInCooldown(ultimateSkill)) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool IsUsingAnySkill()
        {
            if (GetIsUsingSkill(movementSkill)) return true;
            if (GetIsUsingSkill(primarySkill)) return true;
            if (GetIsUsingSkill(ultimateSkill)) return true;
            return false;
        }
        public bool IsInAnySkillAnimation()
        {
            if (IsInSkillAnimation(movementSkill)) return true;
            if (IsInSkillAnimation(primarySkill)) return true;
            if (IsInSkillAnimation(ultimateSkill)) return true;
            return false;
        }
        #endregion

        #region Player Attributes
        public string characterName;

        [FoldoutGroup("Aiming Asset References")]
        public Canvas skillshotCanvas = null;
        [FoldoutGroup("Aiming Asset References")]
        public Canvas reticleCanvas = null;
        [FoldoutGroup("Aiming Asset References")]
        public Image skillshotImage = null;
        [FoldoutGroup("Aiming Asset References")]
        public Image rangeImage = null;
        [FoldoutGroup("Aiming Asset References")]
        public Image reticleImage = null;
        #endregion

        #region Initializations
        public void IntializeCharacter(CharacterScriptableObject character)
        {
            /*  Character Intializations

            Summary: Characters must be swapped in and out during runtime.

            When instantiating a prefab, it won't be connected to Animator
            unless you SetActive off and on again (weird). */
            Instantiate(character.characterPrefab, transform);
            InitializeCharacterStats(character);
            InitializeSkillStats(character);
            actionManager.InitializeCharacterFX(character);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void InitializeCharacterStats(CharacterScriptableObject character)
        {
            this.characterName = character.characterName;
            this.numberOfAutoAttacksHits = character.numberOfAutoAttackHits;
            GenerateAutoAttackArray(numberOfAutoAttacksHits);
            if (!character.animatorOverride) return;
            else animator.runtimeAnimatorController = character.animatorOverride;
        }
        public void InitializeSkillStats(CharacterScriptableObject character)
        {
            skillshotImage.enabled = false;
            rangeImage.enabled = false;
            reticleImage.enabled = false;

            InitializeMovementSkill(character);
            InitializePrimarySkill(character);
            InitializeUltimateSkill(character);
        }

        private void InitializeMovementSkill(CharacterScriptableObject character)
        {
            if (!movementSkill) movementSkill = gameObject.AddComponent<SkillManager>();
            movementSkill.Initialize(gameObject, animator, "movementSkill", character.movementSkill);
        }
        private void InitializePrimarySkill(CharacterScriptableObject character)
        {
            if (!primarySkill) primarySkill = gameObject.AddComponent<SkillManager>();
            primarySkill.Initialize(gameObject, animator, "primarySkill", character.primarySkill);
        }
        private void InitializeUltimateSkill(CharacterScriptableObject character)
        {
            if (!ultimateSkill) ultimateSkill = gameObject.AddComponent<SkillManager>();
            ultimateSkill.Initialize(gameObject, animator, "ultimateSkill", character.ultimateSkill);
        }
        #endregion

        #region Dash Mechanics
        [FoldoutGroup("Dash Mechanics")]
        [SerializeField] float dashSpeed = 20f;
        [FoldoutGroup("Dash Mechanics")]
        public bool isDashing = false;

        [Header("Dash Charges")]
        [FoldoutGroup("Dash Mechanics")]
        [SerializeField] int maxDashCharges = 3;
        [FoldoutGroup("Dash Mechanics")]
        [SerializeField] int currentDashCharges = 3;
        [FoldoutGroup("Dash Mechanics")]
        [SerializeField] float dashRegenRate = 3f;

        public bool GetCanDash()
        {
            return currentDashCharges > 0;
        }

        public bool GetIsDashing()
        {
            return isDashing;
        }

        public void SetIsDashing(bool value)
        {
            isDashing = value;
        }

        public float GetDashSpeed()
        {
            return dashSpeed;
        }

        public void TriggerDash()
        {
            isDashing = true;
            StartCoroutine(RegenDashCharge());
        }

        IEnumerator RegenDashCharge()
        {
            currentDashCharges--;
            yield return new WaitForSeconds(dashRegenRate);
            currentDashCharges++;
        }
        #endregion

        #region Auto Attack Mechanics
        /* On Hold
        [SerializeField] LayerMask enemyLayers;
        [SerializeField] Transform attackHitBoxPoint = default;
        [Range(0f, 2f)]
        [SerializeField] public float attackRange = .5f;
        [SerializeField] float[] damageList = { 5f, 7f }; */
        /*  Auto Attack State Mechanics

            Summary: When you attack, you should be able to cancel out of movement.

            Moving Scenario: While moving, the player should be able to cancel into
            an attack. To do so, we set the isInAutoAttack bool to TRUE. When the attack
            animation completes, we dash out, we set isInAutoAttack bool to FALSE.

            Implementation: To provide them the ability to read & write to the bool, we
            give the auto attack state direct access to the manager.

            We use isInAutoAttackState to prevent override. We do not want the next auto
            attack to override auto attack nor the next auto attack.
        */
        /*  Auto Attack Override Prevention Mechanics

            Summary: Upon attacking, you should not be allowed to override the current attack
            until the current attack is complete. We use a delegated function to get/set a
            bool to prevent this from occurring.

            Combo Scenario: To prevent the override, we use the canTriggerNextAutoAttack bool.
            During an attack animation, we set the bool to FALSE so the player
            is unable to override. When the animation is complete, we set the
            bool to TRUE so that the player has permission to continue the combo.

            Dash Cancel Scenario: If the player is in an Auto Attack animation, he
            must be able to cancel the action with a dash. However, since the animation
            never completes due to the dash cancel, the canTriggerNextAutoAttack never returns
            to TRUE. To resolve this, the AutoAttack state must set the bool back to TRUE
            upon exiting the state machine.

            Implementation: To provide them the ability to read & write to the bool, we
            give the auto attack state direct access to the manager.
         */
        [FoldoutGroup("Auto Attack Mechanics")]
        public int numberOfAutoAttacksHits;
        [FoldoutGroup("Auto Attack Mechanics")]
        [SerializeField] bool isInAutoAttackState = false;
        [FoldoutGroup("Auto Attack Mechanics")]
        [SerializeField] bool canTriggerNextAutoAttack = true;
        [FoldoutGroup("Auto Attack Mechanics")]
        [SerializeField] int comboNum = 0;
        private string[] autoAttackArray = null;


        public void SetIsInAutoAttackState(bool value) => isInAutoAttackState = value;
        public bool GetCanTriggerNextAutoAttack() => canTriggerNextAutoAttack;
        public void SetCanTriggerNextAutoAttack(bool value) => canTriggerNextAutoAttack = value;
        private void GenerateAutoAttackArray(int numOfAutoAttackHits)
        {
            string[] autoAttackTempArray = new string[numOfAutoAttackHits];
            for (int i = 0; i < numOfAutoAttackHits; i++)
            {
                autoAttackTempArray[i] = ("attack" + (i + 1).ToString());
            }
            autoAttackArray = autoAttackTempArray;
        }
        public string[] GetAutoAttackArray()
        {
            return autoAttackArray;
        }
        public bool IsInAutoAttackAnimation()
        {
            for (int i = 0; i < numberOfAutoAttacksHits; i++)
            {
                // We dynamically generate an array with "attack#" values.
                // These values are used to interact with the Animator.
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(autoAttackArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        // Combo Mechanics
        public void SetComboNum(int i) => comboNum = i;
        public int GetComboNum() => comboNum;

        // Auto Attack Animator Events
        public void AttackStart() { }
        public void AttackEnd()
        {
            SetCanTriggerNextAutoAttack(true);
            SetIsInAutoAttackState(false);
        }
        #endregion

        #region Skill Mechanics
        public bool GetIsSkillInCooldown(SkillManager skill)
        {
            return skill.GetIsSkillInCooldown();
        }
        public void SetIsUsingSkill(SkillManager skill, bool value)
        {
            skill.SetIsUsingSkill(value);
        }
        public bool GetIsUsingSkill(SkillManager skill)
        {
            return skill.GetIsUsingSkill();
        }
        public bool IsInSkillAnimation(SkillManager skill)
        {
            return skill.IsInSkillAnimation();
        }
        public bool GetSkillRequiresAim(SkillManager skill)
        {
            return skill.requiresSkillShot ||
                   skill.requiresRangeShot;
        }
        public bool GetAimingEnabled(SkillManager skill)
        {
            return skill.GetAimingEnabled();
        }
        public void SetAimingEnabled(SkillManager skill, bool value)
        {
            skill.isAimingSkill = value;
        }
        public void ActivateSkillAim(SkillManager skill, string skillType)
        {
            skill.SetAimingEnabled(true);
            InitializeAimImage(skillType);
            string aimType = skill.requiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(aimType, true);
        }
        public void DeactivateSkillAim(SkillManager skill)
        {
            skill.SetAimingEnabled(false);
            string skillType = skill.requiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, false);
        }
        public void TriggerSkill(SkillManager skill)
        {
            skill.TriggerSkill();
            DeactivateSkillAim(skill);
        }
        // Animation Event
        public void PrimarySkillActivate()
        {
            primarySkill.SetIsUsingSkill(false);
        }
        public void UltimateSkillActivate()
        {
            ultimateSkill.SetIsUsingSkill(false);
        }
        public void MovementSkillActivate()
        {
            movementSkill.SetIsUsingSkill(false);
        }
        #endregion

        #region Aiming Mechanics
        private bool skillshotAimingActive = false;
        private bool rangeshotAimingActive = false;
        private float maxRangeShotDistance = 5f;

        private string MOVEMENT = "movementSkill";
        private string PRIMARY = "primarySkill";
        private string ULTIMATE = "ultimateSkill";

        private string SKILLSHOT = "SKILLSHOT";
        private string RANGESHOT = "RANGESHOT";

        private void InitializeAimImage(string skillType)
        {
            if (skillType == MOVEMENT)
            {
                if (movementSkill.requiresSkillShot)
                {
                    skillshotImage.sprite = movementSkill.skillShotImage;
                }
                if (movementSkill.requiresRangeShot)
                {
                    rangeImage.sprite = movementSkill.rangeImage;
                    reticleImage.sprite = movementSkill.reticleImage;
                }
            }
            if (skillType == PRIMARY)
            {
                if (primarySkill.requiresSkillShot)
                {
                    skillshotImage.sprite = primarySkill.skillShotImage;
                }
                if (primarySkill.requiresRangeShot)
                {
                    rangeImage.sprite = primarySkill.rangeImage;
                    reticleImage.sprite = primarySkill.reticleImage;
                }
            }
            if (skillType == ULTIMATE)
            {
                if (ultimateSkill.requiresSkillShot)
                {
                    skillshotImage.sprite = ultimateSkill.skillShotImage;
                }
                if (ultimateSkill.requiresRangeShot)
                {
                    rangeImage.sprite = ultimateSkill.rangeImage;
                    reticleImage.sprite = ultimateSkill.reticleImage;
                }
            }
        }
        private void SetAimingEnabled(string skillType, bool value)
        {
            if (skillType == "SKILLSHOT")
            {
                skillshotImage.enabled = value;
                skillshotAimingActive = value;
            }
            if (skillType == "RANGESHOT")
            {
                rangeImage.enabled = value;
                reticleImage.enabled = value;
                rangeshotAimingActive = value;
            }
        }
        public void AimWithSkillshot()
        {
            mousePosition = raycaster.GetRaycastMousePoint().point;
            Quaternion transRot = Quaternion.LookRotation(mousePosition - transform.position);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            skillshotCanvas.transform.rotation = Quaternion.Lerp(transRot, skillshotCanvas.transform.rotation, 0f);
        }
        public void AimWithRangeshot()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();

            var hitPosDir = (hit.point - transform.position).normalized;
            float distance = Vector3.Distance(hit.point, transform.position);
            distance = Mathf.Min(distance, maxRangeShotDistance);

            var newHitPos = transform.position + hitPosDir * distance;
            reticleCanvas.transform.position = newHitPos;
        }
        #endregion
    }
}
