#nullable enable

namespace HalfEmpty.Application.FSM
{
    /// <summary>
    /// Generic state machine. Delegates Update/FixedUpdate to the active state.
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        /// <summary>Transition to a new state.</summary>
        public void ChangeState(IState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}
