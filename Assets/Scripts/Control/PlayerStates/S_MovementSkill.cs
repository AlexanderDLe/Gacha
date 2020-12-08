using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.PlayerStates
{
    public class S_MovementSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        NavMeshAgent navMeshAgent;
        Animator animator = null;
        RaycastMousePosition raycaster = null;
        SkillManager skill;
        Vector3 movement;

        public S_MovementSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, NavMeshAgent navMeshAgent, StateManager stateManager, SkillManager skill)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.navMeshAgent = navMeshAgent;
            this.stateManager = stateManager;
            this.skill = skill;
        }

        public void Enter()
        {
            stateManager.TriggerSkill(skill);
            navMeshAgent.updateRotation = false;
        }

        public void Execute()
        {
            movement = gameObject.transform.forward;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = 10000f;
            navMeshAgent.destination = gameObject.transform.position + movement;

        }

        public void Exit()
        {
            navMeshAgent.updateRotation = true;
            skill.SetIsUsingSkill(false);
            animator.ResetTrigger("movementSkill");
        }
    }
}