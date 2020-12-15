using RPG.Control;
using RPG.Core;

namespace RPG.PlayerStates
{
    public class P_SwapCharacter : IState
    {
        StateManager stateManager;
        int index;

        public P_SwapCharacter(StateManager stateManager, int index)
        {
            this.stateManager = stateManager;
            this.index = index;
        }

        public void Enter()
        {
            stateManager.SwapCharacter(index);
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}