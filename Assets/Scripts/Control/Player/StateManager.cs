using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Attributes;
using RPG.Core;
using RPG.Combat;

namespace RPG.Control
{
    public class StateManager : BaseManager
    {
        #region Management Systems
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        Vector3 mousePosition = Vector3.zero;
        public GameObject environment = null;

        [FoldoutGroup("Management Systems")]
        CharacterBuilder builder = null;
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
        [FoldoutGroup("Management Systems")]
        EffectExecutor effectExecuter = null;
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
            raycaster = GetComponent<RaycastMousePosition>();
            builder = GetComponent<CharacterBuilder>();
            initialize = GetComponent<InitializeManager>();
            audioManager = GetComponent<AudioManager>();
            dasher = GetComponent<DashManager>();
            attacker = GetComponent<AttackManager>();
            aimer = GetComponent<AimManager>();
            effectExecuter = GetComponent<EffectExecutor>();
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
            audioManager.SetAudioSources(characterAudioSource, actionAudioSource);
            attacker.LinkReferences(audioManager, raycaster, animator, objectPooler);
            builder.LinkReferences(animator, objectPooler, audioManager, raycaster);
            dasher.LinkReferences(audioManager);
            aimer.LinkReferences(raycaster);
            raycaster.LinkReferences(objectPooler);
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
            initialize.CharacterStats(out baseStats, out currCharName, out currCharImage);
            initialize.CharacterSkills(out movementSkill, out primarySkill, out ultimateSkill);
            initialize.CharacterAnimation(animator);
            effectExecuter.Initialize(baseStats);
            attacker.Initialize(character, baseStats);
            aimer.Initialize(character);
            dasher.Initialize(character);
            CharacterInitializationComplete();
        }
        #endregion

        #region Functions
        public override void TakeDamage(int damage)
        {
            baseStats.TakeDamage(damage);
        }
        public override void ExecuteEffectPackage(EffectPackage effectPackage)
        {
            print("Execute Effect Package");
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
        public SkillManager ultimateSkill = null;
        [FoldoutGroup("Skills")]
        public SkillManager primarySkill = null;
        [FoldoutGroup("Skills")]
        public SkillManager movementSkill = null;

        public void ActivateSkillAim(SkillManager skill, string skillType)
        {
            skill.SetAimingEnabled(true);
            aimer.InitializeAimImage(skill);
            string aimType = GetSkillType(skill);
            aimer.SetAimingEnabled(aimType, true);
        }
        public void DeactivateSkillAim(SkillManager skill)
        {
            skill.SetAimingEnabled(false);
            string skillType = GetSkillType(skill);
            aimer.SetAimingEnabled(skillType, false);
        }
        private string GetSkillType(SkillManager skill)
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
