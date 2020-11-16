using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class SDasher : IState
    {
        Animator animator = null;
        private float speed = 0f;
        private GameObject gameObject = null;
        private NavMeshAgent navMeshAgent;

        public SDasher(GameObject gameObjectOwner, NavMeshAgent navMeshAgent, Animator animator, float dashSpeed)
        {
            this.speed = dashSpeed;
            this.animator = animator;
            this.gameObject = gameObjectOwner;
            this.navMeshAgent = navMeshAgent;
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
            navMeshAgent.speed = speed * Mathf.Clamp01(speedFraction);
        }

        public void Exit()
        {
            navMeshAgent.isStopped = true;
        }
    }
}