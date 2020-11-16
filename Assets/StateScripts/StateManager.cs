using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    public class StateManager : MonoBehaviour
    {
        #region Intializations
        Animator animator = null;
        FXManager fxManager = null;
        public CharacterScriptableObject currentCharacter = null;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            fxManager = GetComponent<FXManager>();
        }
        private void Start()
        {
            IntializeCharacter(currentCharacter);
            UpdateAnimationOverride(currentCharacter);
            SetCurrentCharacterStats(currentCharacter);
            currentDashCharges = maxDashCharges;
        }
        #endregion

        #region Permissions
        public bool CanMove()
        {
            if (isDashing) return false;
            if (isInAutoAttackState || IsInAutoAttackAnimation()) return false;
            return true;
        }
        public bool CanDash()
        {
            if (currentDashCharges > 0 && !isDashing) return true;
            return false;
        }
        public bool CanAutoAttack()
        {
            if (isDashing) return false;
            if (isInAutoAttackState) return false;
            if (IsInAutoAttackAnimation()) return false;
            return true;
        }

        // public bool CanUsePrimarySkill()
        // {
        //     if (isDashing) return false;

        // }
        #endregion

        #region Player Attributes
        string currentCharacterName;
        public int numberOfAutoAttacksHits;
        #endregion

        #region Character Swap Mechanics
        public void IntializeCharacter(CharacterScriptableObject character)
        {
            /*  Character Intializations

            Summary: Characters must be swapped in and out during runtime.

            When instantiating a prefab, it won't be connected to Animator
            unless you SectActive off and on again (weird). */
            Instantiate(character.characterPrefab, transform);
            fxManager.InitializeCharacterFX(character);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        public void SetCurrentCharacterStats(CharacterScriptableObject character)
        {
            /*  Update Character Stats
            
            With the ability to swap between character, player stats must
            update dynamically during runtime. */
            currentCharacterName = character.characterName;
            numberOfAutoAttacksHits = character.numberOfAutoAttackHits;
            GenerateAutoAttackArray(numberOfAutoAttacksHits);
        }
        private void UpdateAnimationOverride(CharacterScriptableObject character)
        {
            if (!character.animatorOverride) return;
            animator.runtimeAnimatorController = character.animatorOverride;
        }
        #endregion

        #region Dash Mechanics
        [Header("Dash Mechanics")]
        [SerializeField] float dashSpeed = 20f;
        public bool isDashing = false;

        [Header("Dash Charges")]
        [SerializeField] int maxDashCharges = 3;
        [SerializeField] int currentDashCharges = 3;
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
        [Header("Particle Effects")]
        [SerializeField] GameObject[] attackFX = null;
        [SerializeField] float[] damageList = { 5f, 7f }; */
        /*  Auto Attack State Mechanics

            Summary: When you attack, you should be able to cancel out of movement.

            Moving Scenario: While moving, the player should be able to cancel into
            an attack. To do so, we set the isInAutoAttack bool to TRUE. When the attack
            animation completes, we dash out, we set isInAutoAttack bool to FALSE.

            Implementation: To provide them the ability to read & write to the bool, we
            give the auto attack state direct access to the manager.
        */
        [Header("Auto Attack Mechanics")]
        [Tooltip("Enter and leave Auto Attack state.")]
        [SerializeField] bool isInAutoAttackState = false;
        public void SetIsInAutoAttackState(bool value) => isInAutoAttackState = value;
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
        [Tooltip("Can you start next attack?")]
        [SerializeField] bool canTriggerNextAutoAttack = true;
        public bool GetCanTriggerNextAutoAttack() => canTriggerNextAutoAttack;
        public void SetCanTriggerNextAutoAttack(bool value) => canTriggerNextAutoAttack = value;

        // Auto Attack Array
        private string[] autoAttackArray = null;
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
        public int comboNum = 0;
        public void SetComboNum(int i) => comboNum = i;
        public int GetComboNum() => comboNum;

        // Auto Attack Animator Events

        public void AttackStart()
        {
            SetCanTriggerNextAutoAttack(false);
        }
        public void AttackEnd()
        {
            SetCanTriggerNextAutoAttack(true);
            SetIsInAutoAttackState(false);
        }
        #endregion
    }
}
