using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.PlayerStates
{
    public class P_AutoAttack : IState
    {
        private GameObject gameObject = null;
        private Animator animator = null;
        private RaycastMousePosition raycaster;
        private string[] autoAttackArray = null;
        private AttackManager attacker = null;

        [Tooltip("The current index of the combo.")]
        private float comboResetTimer = 0;
        private float timeUntilComboReset = 3;

        bool repeatAction = false;

        public P_AutoAttack(
            GameObject gameObject,
            Animator animator,
            RaycastMousePosition raycaster,
            StateManager stateManager,
            bool repeatAction
        )
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.attacker = stateManager.attacker;
            this.repeatAction = repeatAction;
            this.autoAttackArray = attacker.GetAutoAttackArray();
        }

        public void Enter()
        {
            Debug.Log("Entering Attack");
        }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0) || repeatAction) TriggerAutoAttack();
            UpdateAutoAttackCycle();
        }

        private void UpdateAutoAttackCycle()
        {
            if (attacker.GetComboNum() > 0)
            {
                comboResetTimer += Time.deltaTime;
                if (comboResetTimer >= timeUntilComboReset)
                {
                    ResetAutoAttack();
                    attacker.SetComboNum(0);
                }
                if (attacker.GetComboNum() == autoAttackArray.Length)
                {
                    attacker.SetComboNum(0);
                }
            }
        }

        private void TriggerAutoAttack()
        {
            if (!attacker.GetCanTriggerNextAutoAttack()) return;
            attacker.SetCanTriggerNextAutoAttack(false);

            raycaster.RotateObjectTowardsMousePosition(gameObject);

            int currentComboNum = attacker.GetComboNum();
            animator.SetTrigger(autoAttackArray[currentComboNum]);
            attacker.SetComboNum(currentComboNum + 1);
            comboResetTimer = 0;
        }

        private void ResetAutoAttack()
        {
            for (int i = 0; i < autoAttackArray.Length; i++)
            {
                animator.ResetTrigger(autoAttackArray[i]);
            }
            animator.SetTrigger("resetAttack");
        }

        public void Exit()
        {
            attacker.SetCanTriggerNextAutoAttack(true);
            attacker.SetIsInAutoAttackState(false);
            ResetAutoAttack();
        }
    }
}