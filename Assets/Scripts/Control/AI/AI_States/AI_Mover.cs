using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{
    public class AI_Mover : IState
    {
        NavMeshAgent navMeshAgent;
        GameObject player;
        Vector3 destination;
        float movementSpeed;

        public AI_Mover(GameObject player, NavMeshAgent navMeshAgent, float movementSpeed)
        {
            this.navMeshAgent = navMeshAgent;
            this.player = player;
            this.movementSpeed = movementSpeed;
        }
        public AI_Mover(Vector3 destination, NavMeshAgent navMeshAgent, float movementSpeed)
        {
            this.navMeshAgent = navMeshAgent;
            this.destination = destination;
            this.movementSpeed = movementSpeed;
        }

        public void Enter()
        {
            navMeshAgent.speed = movementSpeed;
            navMeshAgent.isStopped = false;
        }

        public void Execute()
        {
            if (player) destination = player.transform.position;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = movementSpeed;
        }

        public void Exit()
        {
            navMeshAgent.speed = 0f;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }
}