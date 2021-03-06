﻿using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.PlayerStates
{
    public class P_Move : IState
    {
        private GameObject gameObject = null;
        private NavMeshAgent navMeshAgent;
        private float speed = 6f;

        public P_Move(GameObject gameObjectOwner, NavMeshAgent navMeshAgent)
        {
            this.gameObject = gameObjectOwner;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            navMeshAgent.isStopped = false;
        }

        public void Execute()
        {
            // Get movement Input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Detect movement input
            bool shouldMove = Mathf.Abs(movement.x) > Mathf.Epsilon || Mathf.Abs(movement.z) > Mathf.Epsilon;
            if (!shouldMove) return;

            // If there is input, then move
            StartMoveAction(gameObject.transform.position + movement, 1f);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
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
