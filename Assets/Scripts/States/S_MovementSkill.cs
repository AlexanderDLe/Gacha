using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class S_MovementSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        NavMeshAgent navMeshAgent;
        Animator animator = null;
        RaycastMousePosition raycaster = null;
        Vector3 movement;

        public S_MovementSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, NavMeshAgent navMeshAgent, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.navMeshAgent = navMeshAgent;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            stateManager.TriggerMovementSkill();
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
            stateManager.SetIsUsingMovementSkill(false);
            animator.ResetTrigger("movementSkill");
            animator.SetTrigger("resetAttack");
        }
    }
}