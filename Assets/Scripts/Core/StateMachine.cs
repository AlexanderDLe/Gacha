using UnityEngine;

namespace RPG.Core
{
    public class StateMachine : MonoBehaviour
    {
        private IState CurrentState;
        private StateEnum CurrentStateEnum;

        public void changeState(IState newState, StateEnum newStateEnum)
        {
            if (CurrentStateEnum == newStateEnum) return;
            if (CurrentState != null) CurrentState.Exit();

            CurrentStateEnum = newStateEnum;
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void ExecuteStateUpdate()
        {
            if (CurrentState != null) CurrentState.Execute();
        }
    }
}