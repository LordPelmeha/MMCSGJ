#nullable enable
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
namespace HalfEmpty.Application
{
/// <summary>
/// Top-level game-flow state machine. Owns Menu → Playing → Paused → Game Over states.
/// </summary>
[System.Serializable]
public class GameFlowSM
{
    public GameStateMachine? StateMachine { get; private set; }
    /// <summary>Inject a fully constructed machine after state registration.</summary>
    public void Initialise(GameStateMachine machine) => StateMachine = machine;
    /// <summary>Convenience: transition the inner state machine.</summary>
    public void ChangeState(IState newState) => StateMachine?.ChangeState(newState);
}
}
