#nullable enable
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
namespace HalfEmpty.Application
{
/// <summary>
/// Top-level game-flow state machine. Owns Menu → Playing → Paused → Game Over states.
/// </summary>
public class GameFlowSM
{
    public GameStateMachine? StateMachine { get; private set; }
}
}