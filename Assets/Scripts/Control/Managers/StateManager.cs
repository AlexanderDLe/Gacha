using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Control
{
    public class StateManager : MonoBehaviour
    {
        #region Management Systems
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        Vector3 mousePosition = Vector3.zero;

        [FoldoutGroup("Management Systems")]
        ActionManager actionManager = null;
        [FoldoutGroup("Management Systems")]
        CharacterBuilder build = null;
        [FoldoutGroup("Management Systems")]
        public BaseStats baseStats = null;
        [FoldoutGroup("Management Systems")]
        public DashManager dasher = null;
        [FoldoutGroup("Management Systems")]
        public AttackManager attacker = null;
        [FoldoutGroup("Management Systems")]
        public AimManager aimer = null;
        [FoldoutGroup("Management Systems")]
        public InitializeManager initialize = null;
        [FoldoutGroup("Management Systems")]
        public ObjectPooler objectPooler = null;
        #endregion

        #region Audio
        [FoldoutGroup("Audio Sources")]
        public AudioSource characterAudioSource = null;
        [FoldoutGroup("Audio Sources")]
        public AudioSource actionAudioSource = null;
        [FoldoutGroup("Audio Sources")]
        public AudioManager audioPlayer = null;
        #endregion

        #region Character Scripts
        [FoldoutGroup("Character Scripts")]
        public PlayableCharacter_SO char1_SO = null;
        [FoldoutGroup("Character Scripts")]
        public PlayableCharacter_SO char2_SO = null;
        [FoldoutGroup("Character Scripts")]
        public PlayableCharacter_SO char3_SO = null;
        #endregion

        #region Start
        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionManager = GetComponent<ActionManager>();
            raycaster = GetComponent<RaycastMousePosition>();
            build = GetComponent<CharacterBuilder>();
            initialize = GetComponent<InitializeManager>();
            audioPlayer = GetComponent<AudioManager>();
            dasher = GetComponent<DashManager>();
            attacker = GetComponent<AttackManager>();
            aimer = GetComponent<AimManager>();
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }
        private void Start()
        {
            SetUpReferences();
            BuildAllCharacters();
            currentCharacter = GetCharacter(currentCharIndex);
            InitializeCharacter(currentCharacter);
        }
        public void SetUpReferences()
        {
            audioPlayer.SetAudioSources(characterAudioSource, actionAudioSource);
            actionManager.LinkReferences(audioPlayer, raycaster, objectPooler);
            attacker.LinkReferences(audioPlayer, raycaster, animator, objectPooler);
            build.LinkReferences(animator, objectPooler);
            dasher.LinkReferences(audioPlayer);
            aimer.LinkReferences(raycaster);
        }
        #endregion

        #region Initializations
        private void BuildAllCharacters()
        {
            chars[0] = build.BuildCharacter(char_GOs[0], char1_SO, out char_PFs[0]);
            chars[1] = build.BuildCharacter(char_GOs[1], char2_SO, out char_PFs[1]);
            chars[2] = build.BuildCharacter(char_GOs[2], char3_SO, out char_PFs[2]);
        }

        public event Action CharacterInitializationComplete;
        public void InitializeCharacter(CharacterManager character)
        {
            initialize.CurrentCharacter(character);
            initialize.CharacterPrefab(out currentCharPrefab, char_PFs, currentCharIndex);
            initialize.CharacterStats(out baseStats, out currCharName, out currCharImage);
            initialize.CharacterSkills(out movementSkill, out primarySkill, out ultimateSkill);
            initialize.CharacterAnimation(animator);
            actionManager.Initialize(character, baseStats);
            attacker.Initialize(character, baseStats);
            aimer.Initialize(character);
            dasher.Initialize(character);
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

            TransitionCharacter(charIndex);
            InitializeCharacter(currentCharacter);
            StartCoroutine(StartSwapCooldown());
        }

        private void TransitionCharacter(int charIndex)
        {
            currentCharacter.CancelSkillAiming();
            currentCharPrefab.SetActive(false);
            currentCharIndex = charIndex;
            currentCharacter = GetCharacter(charIndex);
            actionManager.ActivateSwapFX();
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
            aimer.InitializeAimImage(skill);
            string aimType = skill.requiresSkillShot ? aimer.SKILLSHOT : aimer.RANGESHOT;
            aimer.SetAimingEnabled(aimType, true);
        }
        public void DeactivateSkillAim(SkillManager skill)
        {
            skill.SetAimingEnabled(false);
            string skillType = skill.requiresSkillShot ? aimer.SKILLSHOT : aimer.RANGESHOT;
            aimer.SetAimingEnabled(skillType, false);
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
    }
}
