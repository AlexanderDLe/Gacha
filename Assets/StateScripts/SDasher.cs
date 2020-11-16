using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;

namespace RPG.Movement
{
    public class SDasher : IState
    {
        Animator animator = null;
        private GameObject gameObject = null;
        private NavMeshAgent navMeshAgent;
        private StateManager stateManager = null;

        public SDasher(
            GameObject gameObject,
            NavMeshAgent navMeshAgent,
            Animator animator,
            StateManager stateManager)
        {
            this.gameObject = gameObject;
            this.navMeshAgent = navMeshAgent;
            this.animator = animator;
            this.stateManager = stateManager;
        }

        public void Enter()
        {
            animator.SetTrigger("dash");
        }

        public void Execute()
        {
            // Get movement Input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Detect movement input
            bool shouldMove = Mathf.Abs(movement.x) > Mathf.Epsilon || Mathf.Abs(movement.z) > Mathf.Epsilon;
            if (!shouldMove) movement = gameObject.transform.forward;


            // If there is input, then move
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
            navMeshAgent.speed = stateManager.GetDashSpeed() * Mathf.Clamp01(speedFraction);
        }

        public void Exit()
        {
            navMeshAgent.isStopped = true;
            stateManager.SetIsDashing(false);
        }
    }
}