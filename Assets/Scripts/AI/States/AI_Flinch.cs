using RPG.Characters;
using RPG.Core;
using UnityEngine;

namespace RPG.AI
{
    public class AI_Flinch : IState
    {
        AIManager AIManager;
        Animator animator;

        public AI_Flinch(AIManager AIManager, Animator animator)
        {
            this.AIManager = AIManager;
            this.animator = animator;
        }

        public void Enter()
        {
            animator.SetTrigger("flinch");
            AIManager.SetIsFlinching(true);
        }

        public void Execute() { }

        public void Exit()
        {
            animator.ResetTrigger("flinch");
        }
    }
}