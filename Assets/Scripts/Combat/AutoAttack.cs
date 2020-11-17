using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class AutoAttack : IState
    {
        private GameObject gameObject = null;
        private Animator animator = null;
        private RaycastMousePosition raycaster;
        private string[] autoAttackArray = null;
        private StateManager stateManager = null;

        [Tooltip("The current index of the combo.")]
        private float comboResetTimer = 0;
        private float timeUntilComboReset = 1;

        public AutoAttack(
            GameObject gameObject,
            Animator animator,
            RaycastMousePosition raycaster,
            StateManager stateManager
        )
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
            this.autoAttackArray = stateManager.GetAutoAttackArray();
        }

        public void Enter() { }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0)) TriggerAutoAttack();
            UpdateAutoAttackCycle();
        }

        private void UpdateAutoAttackCycle()
        {
            if (stateManager.GetComboNum() > 0)
            {
                comboResetTimer += Time.deltaTime;
                if (comboResetTimer >= timeUntilComboReset)
                {
                    ResetAutoAttack();
                    stateManager.SetComboNum(0);
                }
                if (stateManager.GetComboNum() == autoAttackArray.Length)
                {
                    stateManager.SetComboNum(0);
                }
            }
        }

        private void TriggerAutoAttack()
        {
            if (!stateManager.GetCanTriggerNextAutoAttack()) return;
            stateManager.SetCanTriggerNextAutoAttack(false);
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);

            int currentComboNum = stateManager.GetComboNum();
            animator.SetTrigger(autoAttackArray[currentComboNum]);
            stateManager.SetComboNum(currentComboNum + 1);
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
            stateManager.SetCanTriggerNextAutoAttack(true);
            stateManager.SetIsInAutoAttackState(false);
            ResetAutoAttack();
        }
    }
}