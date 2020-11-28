using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.PlayerStates
{
    public class S_UltimateSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        Animator animator = null;
        RaycastMousePosition raycaster = null;
        SkillManager skill;

        public S_UltimateSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, StateManager stateManager, SkillManager skill)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.stateManager = stateManager;
            this.skill = skill;
        }

        public void Enter()
        {
            stateManager.TriggerSkill(skill);
        }

        public void Execute() { }

        public void Exit()
        {
            skill.SetIsUsingSkill(false);
            animator.ResetTrigger("ultimateSkill");
            animator.SetTrigger("resetAttack");
        }
    }
}