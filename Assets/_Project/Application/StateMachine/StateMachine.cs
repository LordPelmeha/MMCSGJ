#nullable enable

namespace HalfEmpty.Application.FSM
{
    /// <summary>
    /// Generic state machine. Delegates Update/FixedUpdate to the active state.
    /// </summary>
    public class StateMachine
    {
        private IState? _currentState;
        /// <summary>Transition to a new state.</summary>
        public void ChangeState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
        /// <summary>Forward Unity Update to the active state.</summary>
        public void Update()
        {
            _currentState?.Update();
        }
        /// <summary>Forward Unity FixedUpdate to the active state.</summary>
        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }
    }
}
