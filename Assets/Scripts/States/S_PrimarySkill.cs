using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class S_PrimarySkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        Animator animator = null;
        RaycastMousePosition raycaster = null;

        public S_PrimarySkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            stateManager.TriggerPrimarySkill();
        }

        public void Execute() { }

        public void Exit()
        {
            stateManager.SetIsUsingPrimarySkill(false);
            animator.ResetTrigger("primarySkill");
            animator.SetTrigger("resetAttack");
        }
    }
}