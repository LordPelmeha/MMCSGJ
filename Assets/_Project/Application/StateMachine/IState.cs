#nullable enable
namespace HalfEmpty.Application.FSM
{
/// <summary>
/// Minimal state interface.
/// </summary>
public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}
}