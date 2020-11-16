using System;
using System.Collections;
using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    public class StateManager : MonoBehaviour
    {
        Animator animator = null;
        public CharacterScriptableObject currentCharacter = null;

        #region Intializations
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            IntializeCharacter(currentCharacter);
            UpdateAnimationOverride(currentCharacter);
            SetCurrentCharacterStats(currentCharacter);
            InitializeDelegates();
            currentDashCharges = maxDashCharges;
        }

        private void InitializeDelegates()
        {
            getCanTriggerNextAutoAttackDelegate = GetCanTriggerNextAutoAttack;
            setCanTriggerAutoAttackDelegate = SetCanTriggerNextAutoAttack;
            setIsInAutoAttackStateDelegate = SetIsInAutoAttackState;
        }
        #endregion

        #region Permissions
        public bool CanDash()
        {
            if (currentDashCharges > 0 && !isDashing) return true;
            return false;
        }
        public bool CanAutoAttack()
        {
            if (isDashing) return false;
            if (isInAutoAttackState) return false;
            return true;
        }
        public bool CanMove()
        {
            if (isDashing) return false;
            if (isInAutoAttackState || IsInAutoAttackAnimation()) return false;
            return true;
        }
        // public bool CanUsePrimarySkill()
        // {
        //     if (isDashing) return false;

        // }
        #endregion

        #region Character Swap Mechanics
        public void IntializeCharacter(CharacterScriptableObject character)
        {
            /*  Character Intializations

            Summary: Characters must be swapped in and out during runtime.

            When instantiating a prefab, it won't be connected to Animator
            unless you SectActive off and on again (weird). */
            Instantiate(character.characterPrefab, transform);
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
        }
        private void UpdateAnimationOverride(CharacterScriptableObject character)
        {
            animator.runtimeAnimatorController = character.animatorOverride;
        }
        #endregion

        #region Attributes
        string currentCharacterName;
        public int numberOfAutoAttacksHits;

        public int GetNumberOfAutoAttackHits()
        {
            return numberOfAutoAttacksHits;
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

        public void TriggerDash()
        {
            isDashing = true;
            StartCoroutine(RegenDashCharge());
        }

        public float GetDashSpeed()
        {
            return dashSpeed;
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
        [Header("Auto Attack Mechanics")]
        [SerializeField] string[] autoAttackAnimList = { "attack1", "attack2" };

        /*  Auto Attack State Mechanics
            
            Summary: When you attack, you should be able to cancel out of movement.
            
            Moving Scenario: While moving, the player should be able to cancel into
            an attack. To do so, we set the isInAutoAttack bool to TRUE. When the attack
            animation completes, we dash out, we set isInAutoAttack bool to FALSE.

            Implementation:To provide them the ability to read & write to the bool, we use delegates to give a reference of the function to the state.
        */
        [Tooltip("Enter and leave Auto Attack state.")]
        [SerializeField] bool isInAutoAttackState = false;
        public delegate void SetIsInAutoAttackDelegate(bool value);
        public SetIsInAutoAttackDelegate setIsInAutoAttackStateDelegate;
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

            Implementation: As it currently stands, the states of the State Machine are
            unable to directly access Animator events. To provide them the ability to
            read & write to the bool, we use delegates to give a reference of the functions
            to the state.
         */
        [Tooltip("Can you start next attack?")]
        [SerializeField] bool canTriggerNextAutoAttack = true;
        // GETTER Delegates
        public delegate bool GetCanAutoAttackDelegate();
        public GetCanAutoAttackDelegate getCanTriggerNextAutoAttackDelegate;
        public bool GetCanTriggerNextAutoAttack() => canTriggerNextAutoAttack;

        // SETTER Delegates
        public delegate void SetCanAutoAttackDelegate(bool value);
        public SetCanAutoAttackDelegate setCanTriggerAutoAttackDelegate;
        public void SetCanTriggerNextAutoAttack(bool value) => canTriggerNextAutoAttack = value;

        public string[] GetAutoAttackAnimList()
        {
            return autoAttackAnimList;
        }
        public bool IsInAutoAttackAnimation()
        {
            foreach (string anim in autoAttackAnimList)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
                {
                    return true;
                }
            }
            return false;
        }

        // Auto Attack Animator Events
        public void AttackStart()
        {
            // Debug.Log("<color=yellow>Attack Starting..</color>");
            canTriggerNextAutoAttack = false;
        }
        public void Attack1()
        {

        }
        public void Attack2()
        {

        }
        public void AttackEnd()
        {
            canTriggerNextAutoAttack = true;
            SetIsInAutoAttackState(false);
        }
        #endregion
    }
}
