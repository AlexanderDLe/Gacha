using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{
    public class AI_Attacker : IState
    {
        GameObject player;
        AIManager AIManager;
        Animator animator;

        public AI_Attacker(GameObject player, AIManager AIManager, Animator animator)
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
                AIManager.attacker.TriggerAttack();
            }
        }

        public void Exit()
        {
            AIManager.attacker.AttackEnd();
            animator.ResetTrigger("attack");
            animator.SetTrigger("resetAttack");
        }
    }
}