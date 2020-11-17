using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class PrimarySkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        Animator animator = null;
        RaycastMousePosition raycaster = null;

        public PrimarySkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            TriggerPrimarySkill();
        }

        public void Execute() { }

        private void TriggerPrimarySkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger("primarySkill");
        }

        public void Exit()
        {
            stateManager.SetIsUsingPrimarySkill(false);
            animator.ResetTrigger("primarySkill");
            animator.SetTrigger("resetAttack");
        }

        // Animator Triggered Events
        private void PrimarySkillStart()
        {
        }

        private void PrimarySkillAttack()
        {
        }

    }
}