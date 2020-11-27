using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Control
{
    public class StateManager : MonoBehaviour
    {
        #region Start
        Animator animator = null;
        ActionManager actionManager = null;
        RaycastMousePosition raycaster = null;
        Vector3 mousePosition = Vector3.zero;
        CharacterBuilder builder = null;
        public BaseStats currBaseStats = null;
        public DashManager dasher = null;
        public InitializeManager initialize = null;

        public PlayableCharacter_SO char1_SO = null;
        public PlayableCharacter_SO char2_SO = null;
        public PlayableCharacter_SO char3_SO = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            raycaster = GetComponent<RaycastMousePosition>();
            builder = GetComponent<CharacterBuilder>();
            initialize = GetComponent<InitializeManager>();
            dasher = GetComponent<DashManager>();
        }
        private void Start()
        {
            BuildAllCharacters();
            currentCharacter = GetCharacter(currentCharIndex);
            InitializeCharacter(currentCharacter);
        }
        private void Update()
        {
            if (skillshotAimingActive) AimWithSkillshot();
            if (rangeshotAimingActive) AimWithRangeshot();
        }
        #endregion

        #region Initializations
        private void BuildAllCharacters()
        {
            chars[0] = builder.BuildCharacter(char_GOs[0], char1_SO, out char_PFs[0]);
            chars[1] = builder.BuildCharacter(char_GOs[1], char2_SO, out char_PFs[1]);
            chars[2] = builder.BuildCharacter(char_GOs[2], char3_SO, out char_PFs[2]);
        }

        public event Action CharacterInitializationComplete;
        public void InitializeCharacter(CharacterManager character)
        {
            initialize.CurrentCharacter(character);

            initialize.CharacterPrefab(out currentCharPrefab, char_PFs, currentCharIndex);

            initialize.CharacterStats(out currBaseStats, out currCharName, out currCharImage, out numberOfAutoAttackHits, out autoAttackArray);

            initialize.CharacterSkills(out movementSprite, out primarySprite, out ultimateSprite, out movementSkill, out primarySkill, out ultimateSkill);

            initialize.CharacterAnimation(animator);

            actionManager.Initialize(character, currBaseStats);

            SetAimImagesEnabled(false);
            CharacterInitializationComplete();
        }
        #endregion

        #region Character Swapping Mechanics
        [FoldoutGroup("Current Character Info")]
        public CharacterManager currentCharacter = null;
        [FoldoutGroup("Current Character Info")]
        public GameObject currentCharPrefab = null;

        [FoldoutGroup("Current Character Info")]
        public string currCharName;
        [FoldoutGroup("Current Character Info")]
        public Sprite currCharImage;

        CharacterManager[] chars = new CharacterManager[3];
        GameObject[] char_GOs = new GameObject[3];
        GameObject[] char_PFs = new GameObject[3];
        int currentCharIndex = 0;

        [FoldoutGroup("Character Swap")]
        [SerializeField] float charSwapCooldownTime = 2f;
        [FoldoutGroup("Character Swap")]
        [SerializeField] bool charSwapInCooldown = false;

        public CharacterManager GetCharacter(int charIndex)
        {
            return chars[charIndex];
        }
        public void SwapCharacter(int charIndex)
        {
            if (charIndex == currentCharIndex) return;
            if (charIndex == 1 && !chars[1]) return;
            if (charIndex == 2 && !chars[2]) return;

            currentCharacter.CancelSkillAiming();
            currentCharPrefab.SetActive(false);
            currentCharIndex = charIndex;
            currentCharacter = GetCharacter(charIndex);

            InitializeCharacter(currentCharacter);
            actionManager.ActivateSwapFX();
            StartCoroutine(StartSwapCooldown());
        }

        IEnumerator StartSwapCooldown()
        {
            charSwapInCooldown = true;
            yield return new WaitForSeconds(charSwapCooldownTime);
            charSwapInCooldown = false;
        }
        #endregion

        #region Permissions
        public bool CanSwapCharacter()
        {
            if (IsDashing()) return false;
            if (charSwapInCooldown) return false;
            if (isInAutoAttackState || IsInAutoAttackAnimation()) return false;
            if (IsUsingAnySkill() || IsInAnySkillAnimation()) return false;
            return true;
        }
        public bool CanMove()
        {
            if (IsDashing()) return false;
            if (isInAutoAttackState || IsInAutoAttackAnimation()) return false;
            if (IsUsingAnySkill() || IsInAnySkillAnimation()) return false;
            return true;
        }
        public bool CanDash()
        {
            if (IsDashing()) return false;
            if (dasher.currentDashCharges == 0) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanAutoAttack()
        {
            if (IsDashing()) return false;
            if (isInAutoAttackState) return false;
            if (IsUsingAnySkill()) return false;
            if (IsInAutoAttackAnimation()) return false;
            return true;
        }
        public bool CanUseMovementSkill()
        {
            if (IsDashing()) return false;
            if (GetIsSkillInCooldown(movementSkill)) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUsePrimarySkill()
        {
            if (IsDashing()) return false;
            if (GetIsSkillInCooldown(primarySkill)) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUseUltimateSkill()
        {
            if (IsDashing()) return false;
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

        #region Dash Mechanics
        public bool GetCanDash()
        {
            return dasher.GetCanDash();
        }
        public bool IsDashing()
        {
            return dasher.GetIsDashing();
        }
        public void SetIsDashing(bool value)
        {
            dasher.SetIsDashing(value);
        }
        public float GetDashSpeed()
        {
            return dasher.GetDashSpeed();
        }
        public void TriggerDash()
        {
            dasher.TriggerDash();
        }
        #endregion

        #region Auto Attack Mechanics        
        /*  Auto Attack State Mechanics

            Summary: When you attack, you should be able to cancel out of movement.

            Moving Scenario: While moving, the player should be able to cancel into an attack. To do so, we set the isInAutoAttack bool to TRUE. When the attack animation completes, we dash out, we set isInAutoAttack bool to FALSE.

            Implementation: To provide them the ability to read & write to the bool, we give the auto attack state direct access to the manager.

            We use isInAutoAttackState to prevent override. We do not want the next auto attack to override auto attack nor the next auto attack.
        */
        /*  Auto Attack Override Prevention Mechanics

            Summary: Upon attacking, you should not be allowed to override the current  
            until the current attack is complete. We use a delegated function to get/set a bool to prevent this from occurring.

            Combo Scenario: To prevent the override, we use the canTriggerNextAutoAttack.  an attack animation, we set the bool to FALSE so the player is unable to override. When the animation is complete, we set the bool to TRUE so that the player has permission to continue the combo.

            Dash Cancel Scenario: If the player is in an Auto Attack animation, he must be able to cancel the action with a dash. However, since the animation completes due to the dash cancel, the canTriggerNextAutoAttack never returns to TRUE. To resolve this, the AutoAttack state must set the bool back to TRUE upon exiting the state machine.

            Implementation: To provide them the ability to read & write to the bool, we give the auto attack state direct access to the manager.
         */
        [FoldoutGroup("Auto Attack Mechanics")]
        public int numberOfAutoAttackHits;
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
        public string[] GetAutoAttackArray()
        {
            return autoAttackArray;
        }
        public bool IsInAutoAttackAnimation()
        {
            for (int i = 0; i < numberOfAutoAttackHits; i++)
            {
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
        [FoldoutGroup("Current Character Info")]
        public SkillManager ultimateSkill = null;
        [FoldoutGroup("Current Character Info")]
        public SkillManager primarySkill = null;
        [FoldoutGroup("Current Character Info")]
        public SkillManager movementSkill = null;


        [FoldoutGroup("Skill UI Icons")]
        public Sprite movementSprite = null;
        [FoldoutGroup("Skill UI Icons")]
        public Sprite primarySprite = null;
        [FoldoutGroup("Skill UI Icons")]
        public Sprite ultimateSprite = null;

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
        public void ActivateSkillAim(SkillManager skill, string skillType)
        {
            skill.SetAimingEnabled(true);
            InitializeAimImage(skill);
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

        private string SKILLSHOT = "SKILLSHOT";
        private string RANGESHOT = "RANGESHOT";

        public bool IsAimingActive()
        {
            return skillshotAimingActive || rangeshotAimingActive;
        }
        public void CancelAiming()
        {
            currentCharacter.CancelSkillAiming();

            skillshotAimingActive = false;
            rangeshotAimingActive = false;
            SetAimImagesEnabled(false);
        }
        private void SetAimImagesEnabled(bool value)
        {
            skillshotImage.enabled = value;
            rangeImage.enabled = value;
            reticleImage.enabled = value;
        }
        private void InitializeAimImage(SkillManager skill)
        {
            if (skill.requiresSkillShot)
            {
                skillshotImage.sprite = skill.skillShotImage;
            }
            if (skill.requiresRangeShot)
            {
                rangeImage.sprite = skill.rangeImage;
                reticleImage.sprite = skill.reticleImage;
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
