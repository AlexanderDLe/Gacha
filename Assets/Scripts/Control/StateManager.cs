using System;
using System.Collections;
using UnityEngine;
using System.Drawing;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            raycaster = GetComponent<RaycastMousePosition>();
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
            if (movementSkillInCooldown) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUsePrimarySkill()
        {
            if (isDashing) return false;
            if (primarySkillInCooldown) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUseUltimateSkill()
        {
            if (isDashing) return false;
            if (ultimateSkillInCooldown) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool IsUsingAnySkill()
        {
            if (GetIsUsingMovementSkill()) return true;
            if (GetIsUsingPrimarySkill()) return true;
            if (GetIsUsingUltimateSkill()) return true;
            return false;
        }
        public bool IsInAnySkillAnimation()
        {
            if (IsInMovementSkillAnimation()) return true;
            if (IsInPrimarySkillAnimation()) return true;
            if (IsInUltimateSkillAnimation()) return true;
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
            this.movementSkill = character.movementSkill;
            movementSkill.Initialize(gameObject, animator, "movementSkill");

            this.movementCountdownResetTime = movementSkill.skillBaseCooldown;
            if (movementSkill.skillUsesSkillShot)
            {
                this.movementRequiresSkillShot = movementSkill.skillUsesSkillShot;
                this.movementSkillShotImage = movementSkill.skillShotImage;
            }
            if (movementSkill.skillUsesRangeShot)
            {
                this.movementRequiresRangeShot = movementSkill.skillUsesRangeShot;
                this.movementRangeImage = movementSkill.skillRangeImage;
                this.movementReticleImage = movementSkill.skillReticleImage;
            }
        }

        private void InitializeUltimateSkill(CharacterScriptableObject character)
        {
            this.ultimateSkill = character.ultimateSkill;
            ultimateSkill.Initialize(gameObject, animator, "ultimateSkill");

            this.ultimateCooldownResetTime = ultimateSkill.skillBaseCooldown;
            if (ultimateSkill.skillUsesSkillShot)
            {
                this.ultimateRequiresSkillShot = ultimateSkill.skillUsesSkillShot;
                this.ultimateSkillShotImage = ultimateSkill.skillShotImage;
            }
            if (ultimateSkill.skillUsesRangeShot)
            {
                this.ultimateRequiresRangeShot = ultimateSkill.skillUsesRangeShot;
                this.ultimateRangeImage = ultimateSkill.skillRangeImage;
                this.ultimateReticleImage = ultimateSkill.skillReticleImage;
            }
        }

        private void InitializePrimarySkill(CharacterScriptableObject character)
        {
            this.primarySkill = character.primarySkill;
            primarySkill.Initialize(gameObject, animator, "primarySkill");

            this.primaryCooldownResetTime = primarySkill.skillBaseCooldown;
            if (primarySkill.skillUsesSkillShot)
            {
                this.primaryRequiresSkillShot = primarySkill.skillUsesSkillShot;
                this.primarySkillShotImage = primarySkill.skillShotImage;
            }
            if (primarySkill.skillUsesRangeShot)
            {
                this.primaryRequiresRangeShot = primarySkill.skillUsesRangeShot;
                this.primaryRangeImage = primarySkill.skillRangeImage;
                this.primaryReticleImage = primarySkill.skillReticleImage;
            }
        }

        CharacterScriptableObject currentChar;
        public void testAbility()
        {
            currentChar.primarySkill.TriggerSkill();
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

        #region Movement Skill Mechanics
        [Header("Movement SKill")]
        SkillScriptableObject movementSkill;
        [FoldoutGroup("Movement Skill Mechanic")]
        [SerializeField] bool isUsingMovementSkill = false;
        [FoldoutGroup("Movement Skill Mechanic")]
        [SerializeField] bool movementSkillInCooldown = false;
        [FoldoutGroup("Movement Skill Mechanic")]
        [SerializeField] float movementCountdownResetTime = 3f;
        [FoldoutGroup("Movement Skill Mechanic")]
        [SerializeField] float movementSkillCountdownTimer = 3f;

        [Header("Movement Skill Aiming")]
        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] Sprite movementSkillShotImage = null;
        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] Sprite movementRangeImage = null;
        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] Sprite movementReticleImage = null;

        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] bool movementRequiresSkillShot = false;
        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] bool movementRequiresRangeShot = false;
        [FoldoutGroup("Movement Skill Mechanics")]
        [SerializeField] bool isAimingMovementSkill = false;
        // private float movementRangeShotMaxDistance = 3f;

        public bool IsMovementSkillInCooldown()
        {
            return movementSkillInCooldown;
        }
        public void SetIsUsingMovementSkill(bool value)
        {
            isUsingMovementSkill = value;
        }
        public bool GetIsUsingMovementSkill()
        {
            return isUsingMovementSkill;
        }
        public bool IsInMovementSkillAnimation()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("movementSkill"))
            {
                return true;
            }
            return false;
        }

        // Movement Skill Aiming
        public bool MovementSkillRequiresAim()
        {
            return movementRequiresSkillShot || movementRequiresRangeShot;
        }
        public bool GetMovementAimingEnabled()
        {
            return isAimingMovementSkill;
        }
        public void SetMovementAimingEnabled(bool value)
        {
            isAimingMovementSkill = value;
        }
        public void ActivateMovementSkillAim()
        {
            SetMovementAimingEnabled(true);
            InitializeAimImage(MOVEMENT);
            string skillType = movementRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, true);
        }
        public void DeactivateMovementSkillAim()
        {
            SetMovementAimingEnabled(false);
            string skillType = movementRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, false);
        }
        public void TriggerMovementSkill()
        {
            movementSkill.TriggerSkill();
            SetIsUsingMovementSkill(true);
            DeactivateMovementSkillAim();
            StartCoroutine(MovementSkillCountdown());
        }
        IEnumerator MovementSkillCountdown()
        {
            movementSkillInCooldown = true;
            movementSkillCountdownTimer = movementCountdownResetTime;
            while (movementSkillCountdownTimer > 0)
            {
                movementSkillCountdownTimer -= Time.deltaTime;
                yield return null;
            }
            movementSkillInCooldown = false;
        }
        public void MovementSkillActivate()
        {
            SetIsUsingMovementSkill(false);
        }
        #endregion

        #region Primary Skill Mechanics
        [Header("Primary Skill Cooldown")]
        SkillScriptableObject primarySkill = null;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] bool isUsingPrimarySkill = false;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] bool primarySkillInCooldown = false;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] float primaryCooldownResetTime = 3f;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] float primarySkillCountdownTimer = 0f;

        [Header("Primary Skill Aiming")]
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] Sprite primarySkillShotImage = null;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] Sprite primaryRangeImage = null;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] Sprite primaryReticleImage = null;

        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] bool primaryRequiresSkillShot = false;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] bool primaryRequiresRangeShot = false;
        [FoldoutGroup("Primary Skill Mechanics")]
        [SerializeField] bool isAimingPrimarySkill = false;
        // private float primaryRangeShotMaxDistance = 3f;

        public bool IsPrimarySkillInCooldown()
        {
            return primarySkillInCooldown;
        }
        public void SetIsUsingPrimarySkill(bool value)
        {
            isUsingPrimarySkill = value;
        }
        public bool GetIsUsingPrimarySkill()
        {
            return isUsingPrimarySkill;
        }
        public bool IsInPrimarySkillAnimation()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("primarySkill"))
            {
                return true;
            }
            return false;
        }

        // Primary Skill Aiming
        public bool PrimarySkillRequiresAim()
        {
            return primaryRequiresSkillShot || primaryRequiresRangeShot;
        }
        public bool GetPrimaryAimingEnabled()
        {
            return isAimingPrimarySkill;
        }
        public void SetPrimaryAimingEnabled(bool value)
        {
            isAimingPrimarySkill = value;
        }
        public void ActivatePrimarySkillAim()
        {
            SetPrimaryAimingEnabled(true);
            InitializeAimImage(PRIMARY);
            string skillType = primaryRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, true);
        }
        public void DeactivatePrimarySkillAim()
        {
            SetPrimaryAimingEnabled(false);
            string skillType = primaryRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, false);
        }
        // Primary Skill Triggers
        public void TriggerPrimarySkill()
        {
            primarySkill.TriggerSkill();
            SetIsUsingPrimarySkill(true);
            DeactivatePrimarySkillAim();
            StartCoroutine(PrimarySkillCountdown());
        }
        IEnumerator PrimarySkillCountdown()
        {
            primarySkillInCooldown = true;
            primarySkillCountdownTimer = primaryCooldownResetTime;
            while (primarySkillCountdownTimer > 0)
            {
                primarySkillCountdownTimer -= Time.deltaTime;
                yield return null;
            }
            primarySkillInCooldown = false;
        }
        public void PrimarySkillActivate()
        {
            SetIsUsingPrimarySkill(false);
        }
        #endregion

        #region Ultimate Skill Mechanics
        [Header("Ultimate Skill Cooldown")]
        SkillScriptableObject ultimateSkill = null;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] bool isUsingUltimateSkill = false;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] bool ultimateSkillInCooldown = false;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] float ultimateCooldownResetTime = 3f;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] float ultimateSkillCountdownTimer = 0f;

        [Header("Ultimate Skill Aim Status")]
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] bool ultimateRequiresSkillShot = false;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] bool ultimateRequiresRangeShot = false;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] bool isAimingUltimateSkill = false;

        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] Sprite ultimateSkillShotImage = null;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] Sprite ultimateRangeImage = null;
        [FoldoutGroup("Ultimate Skill Mechanics")]
        [SerializeField] Sprite ultimateReticleImage = null;
        // private float ultimateRangeShotMaxDistance = 3f;

        public bool IsUltimateSkillInCooldown()
        {
            return ultimateSkillInCooldown;
        }
        public void SetIsUsingUltimateSkill(bool value)
        {
            isUsingUltimateSkill = value;
        }
        public bool GetIsUsingUltimateSkill()
        {
            return isUsingUltimateSkill;
        }
        public bool IsInUltimateSkillAnimation()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ultimateSkill"))
            {
                return true;
            }
            return false;
        }

        // Ultimate Skill Aiming
        public bool UltimateSkillRequiresAim()
        {
            return ultimateRequiresSkillShot || ultimateRequiresRangeShot;
        }
        public bool GetUltimateAimingEnabled()
        {
            return isAimingUltimateSkill;
        }
        public void SetUltimateAimingEnabled(bool value)
        {
            isAimingUltimateSkill = value;
        }
        public void ActivateUltimateSkillAim()
        {
            SetUltimateAimingEnabled(true);
            InitializeAimImage(ULTIMATE);
            string skillType = ultimateRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, true);
        }
        public void DeactivateUltimateSkillAim()
        {
            SetUltimateAimingEnabled(false);
            string skillType = ultimateRequiresSkillShot ? SKILLSHOT : RANGESHOT;
            SetAimingEnabled(skillType, false);
        }

        // Ultimate Skill Triggers
        public void TriggerUltimateSkill()
        {
            ultimateSkill.TriggerSkill();
            SetIsUsingUltimateSkill(true);
            DeactivateUltimateSkillAim();
            StartCoroutine(UltimateSkillCountdown());
        }
        IEnumerator UltimateSkillCountdown()
        {
            ultimateSkillInCooldown = true;
            ultimateSkillCountdownTimer = ultimateCooldownResetTime;
            while (ultimateSkillCountdownTimer > 0)
            {
                ultimateSkillCountdownTimer -= Time.deltaTime;
                yield return null;
            }
            ultimateSkillInCooldown = false;
        }
        public void UltimateSkillActivate()
        {
            SetIsUsingUltimateSkill(false);
        }
        #endregion

        #region Aiming Mechanics
        private bool skillshotAimingActive = false;
        private bool rangeshotAimingActive = false;
        private float maxRangeShotDistance = 5f;

        private string MOVEMENT = "MOVEMENT";
        private string PRIMARY = "PRIMARY";
        private string ULTIMATE = "ULTIMATE";

        private string SKILLSHOT = "SKILLSHOT";
        private string RANGESHOT = "RANGESHOT";


        private void InitializeAimImage(string skillType)
        {
            if (skillType == MOVEMENT)
            {
                if (movementRequiresSkillShot)
                {
                    skillshotImage.sprite = movementSkillShotImage;
                }
                if (movementRequiresRangeShot)
                {
                    rangeImage.sprite = movementRangeImage;
                    reticleImage.sprite = movementReticleImage;
                }
            }
            if (skillType == PRIMARY)
            {
                if (primaryRequiresSkillShot)
                {
                    skillshotImage.sprite = primarySkillShotImage;
                }
                if (primaryRequiresRangeShot)
                {
                    rangeImage.sprite = primaryRangeImage;
                    reticleImage.sprite = primaryReticleImage;
                }
            }
            if (skillType == ULTIMATE)
            {
                if (ultimateRequiresSkillShot)
                {
                    skillshotImage.sprite = ultimateSkillShotImage;
                }
                if (ultimateRequiresRangeShot)
                {
                    rangeImage.sprite = ultimateRangeImage;
                    reticleImage.sprite = ultimateReticleImage;
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
