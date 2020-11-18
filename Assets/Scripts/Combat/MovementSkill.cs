using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class MovementSkill : IState
    {
        GameObject gameObject;
        StateManager stateManager;
        NavMeshAgent navMeshAgent;
        Animator animator = null;
        RaycastMousePosition raycaster = null;
        Vector3 movement;

        public MovementSkill(GameObject gameObject, Animator animator, RaycastMousePosition raycaster, NavMeshAgent navMeshAgent, StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.animator = animator;
            this.raycaster = raycaster;
            this.navMeshAgent = navMeshAgent;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            TriggerMovementSkill();
        }

        private void TriggerMovementSkill()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger("movementSkill");
        }

        public void Execute()
        {
            movement = gameObject.transform.forward;
            StartMoveAction(gameObject.transform.position + movement, 1f);
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = 10000f;
            navMeshAgent.destination = destination;
        }

        public void Exit()
        {
            stateManager.SetIsUsingMovementSkill(false);
            animator.ResetTrigger("movementSkill");
            animator.SetTrigger("resetAttack");
        }
    }
}