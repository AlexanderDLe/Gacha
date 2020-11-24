using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AIControl
{
    public class AI_Attacker : IState
    {
        GameObject player;
        EnemyAIManager AIManager;
        Animator animator;

        public AI_Attacker(GameObject player, EnemyAIManager AIManager, Animator animator)
        {
            this.player = player;
            this.AIManager = AIManager;
            this.animator = animator;
        }

        public void Enter() { }

        public void Execute()
        {
            AIManager.transform.LookAt(player.transform.position);
            if (AIManager.CanAttack())
            {
                animator.SetTrigger("attack");
                AIManager.TriggerAttack();
            }
        }

        public void Exit()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("resetAttack");
        }
    }
}