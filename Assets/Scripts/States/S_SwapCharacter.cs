using RPG.Core;

namespace RPG.States
{
    public class S_SwapCharacter : IState
    {
        StateManager stateManager;
        int index;

        public S_SwapCharacter(StateManager stateManager, int index)
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