﻿using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Core;
using RPG.Combat;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerManager : BaseManager
    {
        #region Management Systems
        RaycastMousePosition raycaster = null;
        NavMeshAgent navMeshAgent = null;
        Vector3 mousePosition = Vector3.zero;
        GameObject environment = null;

        [FoldoutGroup("Management Systems")]
        CharacterBuilder builder = null;
        [FoldoutGroup("Management Systems")]
        public DashManager dasher = null;
        [FoldoutGroup("Management Systems")]
        public AttackManager attacker = null;
        [FoldoutGroup("Management Systems")]
        public AimManager aimer = null;
        [FoldoutGroup("Management Systems")]
        public InitializeManager initialize = null;
        #endregion

        #region Audio
        [FoldoutGroup("Audio Sources")]
        public AudioSource characterAudioSource = null;
        [FoldoutGroup("Audio Sources")]
        public AudioSource actionAudioSource = null;
        [FoldoutGroup("Audio Sources")]
        public AudioManager audioManager = null;
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
            navMeshAgent = GetComponent<NavMeshAgent>();
            raycaster = GetComponent<RaycastMousePosition>();
            builder = GetComponent<CharacterBuilder>();
            initialize = GetComponent<InitializeManager>();
            audioManager = GetComponent<AudioManager>();
            dasher = GetComponent<DashManager>();
            attacker = GetComponent<AttackManager>();
            aimer = GetComponent<AimManager>();
            effectExecuter = GetComponent<EffectExecutor>();
            projectileLauncher = GetComponent<ProjectileLauncher>();
            environment = GameObject.FindWithTag("Environment");
            debugPooler = GameObject.FindWithTag("DebugPooler").GetComponent<ObjectPooler>();
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
            audioManager.SetAudioSources(characterAudioSource, actionAudioSource);
            attacker.LinkReferences(audioManager, raycaster, animator, projectileLauncher);
            builder.LinkReferences(animator, navMeshAgent, audioManager, raycaster, projectileLauncher);
            dasher.LinkReferences(audioManager);
            aimer.LinkReferences(raycaster);
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
            initialize.CharacterStats(out stats, out currCharName, out currCharImage);
            initialize.CharacterSkills(out movementSkill, out primarySkill, out ultimateSkill);
            initialize.CharacterSkillEventHandler(out skillEventHandler);
            initialize.CharacterAnimation(animator);
            attacker.Initialize(character, stats);
            aimer.Initialize(character);
            dasher.Initialize(character);
            CharacterInitializationComplete();
        }
        #endregion

        #region Functions
        public override void TakeDamage(int damage)
        {
            stats.TakeDamage(damage);
        }
        #endregion

        #region Character Roster Mechanics
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
        [FoldoutGroup("FX")]
        [SerializeField] GameObject swapVisualFX = null;
        [FoldoutGroup("FX")]
        [SerializeField] AudioClip swapAudioFX = null;

        public CharacterManager GetCharacter(int charIndex)
        {
            return chars[charIndex];
        }
        public void SwapCharacter(int charIndex)
        {
            if (charIndex == currentCharIndex) return;
            if (charIndex == 1 && !chars[1]) return;
            if (charIndex == 2 && !chars[2]) return;

            DeinitializeCharacter(charIndex);
            InitializeCharacter(currentCharacter);
            StartCoroutine(StartSwapCooldown());
        }

        private void DeinitializeCharacter(int charIndex)
        {
            currentCharacter.CancelSkillAiming();
            currentCharPrefab.SetActive(false);
            currentCharIndex = charIndex;
            currentCharacter = GetCharacter(charIndex);
            ActivateSwapFX();
        }
        public void ActivateSwapFX()
        {
            GameObject swapVFX = Instantiate(swapVisualFX, transform);
            swapVFX.transform.SetParent(environment.transform);
            audioManager.PlayAudio(AudioEnum.Character, swapAudioFX);
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
            if (movementSkill.IsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUsePrimarySkill()
        {
            if (dasher.IsDashing()) return false;
            if (primarySkill.IsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool CanUseUltimateSkill()
        {
            if (dasher.IsDashing()) return false;
            if (ultimateSkill.IsSkillInCooldown()) return false;
            if (IsUsingAnySkill()) return false;
            return true;
        }
        public bool IsUsingAnySkill()
        {
            if (movementSkill.IsUsingSkill()) return true;
            if (primarySkill.IsUsingSkill()) return true;
            if (ultimateSkill.IsUsingSkill()) return true;
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
        public SkillEventHandler skillEventHandler = null;
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
            string aimType = GetAimType(skill);
            aimer.SetAimingEnabled(aimType, true);
        }
        public void DeactivateSkillAim(SkillManager skill)
        {
            skill.SetAimingEnabled(false);
            string aimType = GetAimType(skill);
            aimer.SetAimingEnabled(aimType, false);
        }
        private string GetAimType(SkillManager skill)
        {
            return skill.requiresSkillShot ? aimer.SKILLSHOT : aimer.RANGESHOT;
        }

        // Animation Events
        public void MovementSkillTriggered() => movementSkill.SetIsUsingSkill(false);
        public void PrimarySkillTriggered() => primarySkill.SetIsUsingSkill(false);
        public void UltimateSkillTriggered() => ultimateSkill.SetIsUsingSkill(false);
        #endregion

        #region Footsteps
        [FoldoutGroup("FX")]
        [SerializeField] AudioClip[] footstepClips = default;

        private void PlayRandomFootstepClip()
        {
            int num = UnityEngine.Random.Range(0, footstepClips.Length);
            audioManager.PlayAudio(AudioEnum.Character, footstepClips[num]);
        }
        private void Footstep()
        {
            PlayRandomFootstepClip();
        }
        #endregion
    }
}
