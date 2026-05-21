#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Input
{
/// <summary>
/// Abstraction over all player input. Allows swapping the real implementation for tests.
/// </summary>
public interface IInputProvider
{
    float HorizontalAxis { get; }
    bool JumpPressed { get; }
    bool DashPressed { get; }
    bool ShootPressed { get; }
    bool ParryPressed { get; }
    bool MarkPressed { get; }
    bool SwitchFormPressed { get; }
    bool SwitchFormReleased { get; }
    bool PausePressed { get; }
    Vector2 MouseWorldPosition { get; }
}
}