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
        private string[] animList = null;

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
            string[] animList,
            GetCanAutoAttackDelegate getCanAutoAttackDelegate,
            SetCanAutoAttackDelegate setCanAutoAttackDelegate,
            SetIsInAutoAttackDelegate setIsInAutoAttackDelegate
        )
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.animList = animList;
            this.raycaster = raycaster;
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
                if (comboNum == animList.Length)
                {
                    comboNum = 0;
                }
            }
        }

        private void TriggerAutoAttack()
        {
            if (!getCanAutoAttackDelegate()) return;
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger(animList[comboNum]);
            comboNum++;
            comboResetTimer = 0;
        }

        private void ResetAutoAttack()
        {
            foreach (string anim in animList)
            {
                animator.ResetTrigger(anim);
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