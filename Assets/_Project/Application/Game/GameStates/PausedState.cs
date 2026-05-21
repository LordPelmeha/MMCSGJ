#nullable enable
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Game
{
/// <summary>
/// Paused game state.
/// </summary>
public class PausedState : IState
{
    public void Enter() { }
    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}
}