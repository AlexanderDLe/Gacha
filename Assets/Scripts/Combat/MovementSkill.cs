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
            Vector3 movement = gameObject.transform.forward;
            StartMoveAction(gameObject.transform.position + movement, 1f);
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = 10000f * Mathf.Clamp01(speedFraction);
        }


        public void Exit()
        {
            stateManager.SetIsUsingMovementSkill(false);
            animator.ResetTrigger("movementSkill");
            animator.SetTrigger("resetAttack");
        }

        // Animator Triggered Events
        private void MovementSkillStart()
        {
        }

        private void MovementSkillAttack()
        {
        }

    }
}