using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class SMover : IState
    {
        [SerializeField] float speed = 6f;
        private GameObject gameObjectOwner = null;
        private NavMeshAgent navMeshAgent;

        public SMover(GameObject gameObjectOwner, NavMeshAgent navMeshAgent)
        {
            this.gameObjectOwner = gameObjectOwner;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter() { }

        public void Execute()
        {
            // Get movement Input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Detect movement input
            bool shouldMove = Mathf.Abs(movement.x) > Mathf.Epsilon || Mathf.Abs(movement.z) > Mathf.Epsilon;
            if (!shouldMove) return;

            // If there is input, then move
            StartMoveAction(gameObjectOwner.transform.position + movement, 1f);
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
            navMeshAgent.speed = 0f;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }
}
