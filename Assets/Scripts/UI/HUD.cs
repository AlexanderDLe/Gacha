using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HUD : MonoBehaviour
    {
        public StateManager stateManager = null;

        [Header("Character Attributes")]
        public Text characterName = null;
        public Text characterHealth = null;
        public Text dashCharges = null;

        [Header("CharacterImage")]
        public Image characterImage;

        [Header("Skill Images")]
        public Image movementSkillImage;
        public Image primarySkillImage;
        public Image ultimateSkillImage;

        [Header("Skill Image Countdown Text")]
        public Text movementText;
        public Text primaryText;
        public Text ultimateText;

        [Header("Skill Image Mask")]
        public Image movementMask;
        public Image primaryMask;
        public Image ultimateMask;

        private bool Initialized = false;

        private void Start()
        {
            InitializeCharacterUI();
            stateManager.OnCharacterInitialization += InitializeCharacterUI;
            stateManager.OnCharacterInitialization += SetIntialized;
            stateManager.OnDashUpdate += UpdateDashCount;
        }

        private void Update()
        {
            if (!Initialized) return;
            UpdateDashCount();
            PollSkillCountdown(stateManager.movementSkill, movementMask, movementText);
            PollSkillCountdown(stateManager.primarySkill, primaryMask, primaryText);
            PollSkillCountdown(stateManager.ultimateSkill, ultimateMask, ultimateText);
        }

        private void UpdateDashCount()
        {
            this.dashCharges.text = "Dash Charges: " + stateManager.currentDashCharges;
        }

        private void PollSkillCountdown(SkillManager skill, Image mask, Text text)
        {
            if (skill.GetIsSkillInCooldown())
            {
                mask.enabled = true;
                text.enabled = true;
                float countdown = skill.skillCountdownTimer;
                text.text = (Mathf.Round(countdown * 10) / 10).ToString();
            }
            else
            {
                mask.enabled = false;
                text.enabled = false;
            }
        }

        public void InitializeCharacterUI()
        {
            movementMask.enabled = false;
            primaryMask.enabled = false;
            ultimateMask.enabled = false;

            movementText.enabled = false;
            primaryText.enabled = false;
            ultimateText.enabled = false;

            this.characterName.text = stateManager.currCharName;
            this.characterHealth.text = stateManager.currCharHealth.ToString();

            characterImage.sprite = stateManager.currCharImage;

            movementSkillImage.sprite = stateManager.movementSprite;
            primarySkillImage.sprite = stateManager.primarySprite;
            ultimateSkillImage.sprite = stateManager.ultimateSprite;
        }

        public void SetIntialized()
        {
            Initialized = true;
        }
    }
}