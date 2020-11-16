using System;
using UnityEngine;

namespace RPG.Core
{
    public class StateMachine : MonoBehaviour
    {
        private IState CurrentState;
        private IState PreviousState;
        private StateEnum CurrentStateEnum;

        public void changeState(IState newState, StateEnum newStateEnum)
        {
            if (CurrentStateEnum == newStateEnum) return;
            if (CurrentState != null) CurrentState.Exit();

            CurrentStateEnum = newStateEnum;
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void ExecuteStateUpdate()
        {
            var RunningState = CurrentState;
            if (CurrentState != null) CurrentState.Execute();
        }

        public void SwitchToPreviousState()
        {
            CurrentState.Exit();
            CurrentState = PreviousState;
            PreviousState.Enter();
        }
    }
}