using RPG.Core;
using UnityEngine;
using static RPG.Control.StateManager;

namespace RPG.Combat
{
    public class SAutoAttack : IState
    {
        private GameObject gameObject = null;
        private Animator animator = null;
        private RaycastMousePosition raycaster;
        private int numberOfAutoAttackHits = 0;

        [Tooltip("The current index of the combo.")]
        [SerializeField] int comboNum = 0;
        private float comboResetTimer = 0;
        private float timeUntilComboReset = 1;

        GetCanAutoAttackDelegate getCanAutoAttackDelegate;
        SetCanAutoAttackDelegate setCanAutoAttackDelegate;
        SetIsInAutoAttackDelegate setIsInAutoAttackDelegate;

        public SAutoAttack(
            GameObject gameObject,
            Animator animator,
            RaycastMousePosition raycaster,
            int NumberOfAutoAttackHits,
            GetCanAutoAttackDelegate getCanAutoAttackDelegate,
            SetCanAutoAttackDelegate setCanAutoAttackDelegate,
            SetIsInAutoAttackDelegate setIsInAutoAttackDelegate
        )
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.numberOfAutoAttackHits = NumberOfAutoAttackHits;
            this.getCanAutoAttackDelegate = getCanAutoAttackDelegate;
            this.setCanAutoAttackDelegate = setCanAutoAttackDelegate;
            this.setIsInAutoAttackDelegate = setIsInAutoAttackDelegate;
        }

        public void Enter() { }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0)) TriggerAutoAttack();
            UpdateAutoAttackCycle();
        }

        private void UpdateAutoAttackCycle()
        {
            if (comboNum > 0)
            {
                comboResetTimer += Time.deltaTime;
                if (comboResetTimer >= timeUntilComboReset)
                {
                    ResetAutoAttack();
                    comboNum = 0;
                }
                if (comboNum == numberOfAutoAttackHits)
                {
                    comboNum = 0;
                }
            }
        }

        private void TriggerAutoAttack()
        {
            if (!getCanAutoAttackDelegate()) return;
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            Debug.Log(hit.point);
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger(GenerateAttackString(comboNum));
            comboNum++;
            comboResetTimer = 0;
        }

        private string GenerateAttackString(int numInCombo)
        {
            /* 
                We add 1 to comboNum because comboNum starts at 0
                while Animator attack triggers start at 1.
             */
            return "attack" + (numInCombo + 1).ToString();
        }

        private void ResetAutoAttack()
        {
            for (int i = 0; i < numberOfAutoAttackHits; i++)
            {
                animator.ResetTrigger(GenerateAttackString(i));
            }
            animator.SetTrigger("resetAttack");
        }

        public void Exit()
        {
            // Debug.Log("<color=red>Auto Attack Exiting...</color>");
            comboNum = 0;
            setCanAutoAttackDelegate(true);
            setIsInAutoAttackDelegate(false);
            ResetAutoAttack();
        }
    }
}