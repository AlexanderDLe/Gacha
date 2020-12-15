using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{
    public class AI_CombatStance : IState
    {
        Animator animator;
        NavMeshAgent navMeshAgent;

        public AI_CombatStance(NavMeshAgent navMeshAgent, Animator animator)
        {
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            animator.SetBool("combatStance", true);
        }

        public void Execute() { }

        public void Exit()
        {

            animator.SetBool("combatStance", false);
        }
    }
}