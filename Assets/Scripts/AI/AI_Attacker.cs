using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AIControl
{
    public class AI_Attacker : IState
    {
        GameObject player;

        public AI_Attacker(GameObject player)
        {
            this.player = player;
        }

        public void Enter()
        {
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }
    }
}