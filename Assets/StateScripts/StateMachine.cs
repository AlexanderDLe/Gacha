using UnityEngine;

namespace RPG.Core
{
    public class StateMachine : MonoBehaviour
    {
        private IState CurrentState;
        private IState PreviousState;

        public void changeState(IState newState)
        {
            if (CurrentState != null) CurrentState.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void ExecuteStateUpdate()
        {
            var RunningState = CurrentState;
            if (RunningState != null) CurrentState.Execute();
        }

        public void SwitchToPreviousState()
        {
            CurrentState.Exit();
            CurrentState = PreviousState;
            PreviousState.Enter();
        }
    }
}