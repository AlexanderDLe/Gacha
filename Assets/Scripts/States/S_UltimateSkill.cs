using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class S_UltimateSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        Animator animator = null;
        RaycastMousePosition raycaster = null;

        public S_UltimateSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            stateManager.TriggerUltimateSkill();
        }

        public void Execute() { }

        public void Exit()
        {
            stateManager.SetIsUsingUltimateSkill(false);
            animator.ResetTrigger("ultimateSkill");
            animator.SetTrigger("resetAttack");
        }
    }
}