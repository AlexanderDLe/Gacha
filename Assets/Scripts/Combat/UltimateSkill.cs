using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class UltimateSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        Animator animator = null;
        RaycastMousePosition raycaster = null;

        public UltimateSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            TriggerUltimateSkill();
        }

        public void Execute() { }

        private void TriggerUltimateSkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger("ultimateSkill");
        }

        public void Exit()
        {
            stateManager.SetIsUsingUltimateSkill(false);
            animator.ResetTrigger("ultimateSkill");
            animator.SetTrigger("resetAttack");
        }
    }
}