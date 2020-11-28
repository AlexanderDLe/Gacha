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
        public AttackManager attacker = null;
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
            attacker = GetComponent<AttackManager>();
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

            initialize.CharacterStats(out currBaseStats, out currCharName, out currCharImage);

            initialize.CharacterSkills(out movementSkill, out primarySkill, out ultimateSkill);

            initialize.CharacterAnimation(animator);

            actionManager.Initialize(character, currBaseStats);

            attacker.Initialize(character);

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
            if (dasher.IsDashing()) return false;
            if (charSwapInCooldown) return false;
            if (attacker.isInAutoAttackState) return false;
            if (attacker.IsInAutoAttackAnimation()) return false;
            if (IsUsingAnySkill() || IsInAnySkillAnimation()) return false;
            return true;
        }
        public bool CanMove()
        {
            if (dasher.IsDashing()) return false;
            if (attacker.isInAutoAttackState) return false;
            if (attacker.IsInAutoAttackAnimation()) return false;
            if (IsUsingAnySkill() || IsInAnySkillAnimation()) return false;
            return true;
        }
        public bool CanDash()
        {
            if (dasher.IsDashing()) return false;
            if (dasher.currentDashCharges == 0) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanAutoAttack()
        {
            if (dasher.IsDashing()) return false;
            if (attacker.isInAutoAttackState) return false;
            if (IsUsingAnySkill()) return false;
            if (attacker.IsInAutoAttackAnimation()) return false;
            return true;
        }
        public bool CanUseMovementSkill()
        {
            if (dasher.IsDashing()) return false;
            if (movementSkill.GetIsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUsePrimarySkill()
        {
            if (dasher.IsDashing()) return false;
            if (primarySkill.GetIsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUseUltimateSkill()
        {
            if (dasher.IsDashing()) return false;
            if (ultimateSkill.GetIsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool IsUsingAnySkill()
        {
            if (movementSkill.GetIsUsingSkill()) return true;
            if (primarySkill.GetIsUsingSkill()) return true;
            if (ultimateSkill.GetIsUsingSkill()) return true;
            return false;
        }
        public bool IsInAnySkillAnimation()
        {
            if (movementSkill.IsInSkillAnimation()) return true;
            if (primarySkill.IsInSkillAnimation()) return true;
            if (ultimateSkill.IsInSkillAnimation()) return true;
            return false;
        }
        #endregion

        #region Skill Mechanics
        [FoldoutGroup("Skills")]
        public SkillManager ultimateSkill = null;
        [FoldoutGroup("Skills")]
        public SkillManager primarySkill = null;
        [FoldoutGroup("Skills")]
        public SkillManager movementSkill = null;

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
