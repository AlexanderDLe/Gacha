using RPG.Control;
using RPG.Core;

namespace RPG.PlayerStates
{
    public class P_SwapCharacter : IState
    {
        PlayerManager playerManager;
        int index;

        public P_SwapCharacter(PlayerManager playerManager, int index)
        {
            this.playerManager = playerManager;
            this.index = index;
        }

        public void Enter()
        {
            playerManager.SwapCharacter(index);
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}