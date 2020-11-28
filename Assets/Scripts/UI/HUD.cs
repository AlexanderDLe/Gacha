using System;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HUD : MonoBehaviour
    {
        public StateManager stateManager = null;
        public BaseStats baseStats = null;
        private bool pollingSkills = false;

        [Header("Character Attributes")]
        public Text characterName = null;
        public Text characterLevel = null;
        public Text characterHealth = null;
        public RectTransform healthBar = null;
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

        private void Start()
        {
            stateManager.CharacterInitializationComplete += InitializeCharacterUI;
            stateManager.CharacterInitializationComplete += UpdateCurrentHealth;
            stateManager.CharacterInitializationComplete += UpdateDashCount;
            stateManager.dasher.OnDashUpdate += UpdateDashCount;
            characterLevel.text = "Level 1";
        }

        public void InitializeCharacterUI()
        {
            baseStats = stateManager.currBaseStats;
            baseStats.OnHealthChange += UpdateCurrentHealth;

            movementMask.enabled = false;
            primaryMask.enabled = false;
            ultimateMask.enabled = false;

            movementText.enabled = false;
            primaryText.enabled = false;
            ultimateText.enabled = false;

            characterName.text = stateManager.currCharName;
            characterImage.sprite = stateManager.currCharImage;

            movementSkillImage.sprite = stateManager.movementSkill.skillImage;
            primarySkillImage.sprite = stateManager.primarySkill.skillImage;
            ultimateSkillImage.sprite = stateManager.ultimateSkill.skillImage;

            if (!pollingSkills)
            {
                InvokeRepeating("PollSkillCooldowns", .1f, .1f);
                pollingSkills = true;
            }
        }

        private void UpdateCurrentHealth()
        {
            float currentHealth = baseStats.currentHealth;
            float maxHealth = baseStats.maxHealth;
            float healthPercentage = baseStats.GetHealthFraction();

            characterHealth.text = currentHealth + "/" + maxHealth;
            healthBar.localScale = new Vector3(healthPercentage, 1, 1);
        }

        private void UpdateCurrentLevel()
        {
            UpdateCurrentHealth();
        }

        private void UpdateDashCount()
        {
            dashCharges.text = "Dash Charges: " + stateManager.dasher.currentDashCharges;
        }

        private void PollSkillCooldowns()
        {
            PollSkillCountdown(stateManager.movementSkill, movementMask, movementText);
            PollSkillCountdown(stateManager.primarySkill, primaryMask, primaryText);
            PollSkillCountdown(stateManager.ultimateSkill, ultimateMask, ultimateText);
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
    }
}