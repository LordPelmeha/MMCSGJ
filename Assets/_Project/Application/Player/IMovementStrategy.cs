#nullable enable
using UnityEngine;
namespace HalfEmpty.Application.Player
{
/// <summary>
/// Strategy for movement that can be swapped when the player changes form.
/// </summary>
public interface IMovementStrategy
{
    /// <summary>Move in the given direction at the configured speed.</summary>
    void Move(Rigidbody2D rb, float direction, float speed);
    /// <summary>True if jumping is enabled for the current form.</summary>
    bool CanJump { get; }
    /// <summary>Apply an upward impulse.</summary>
    void Jump(Rigidbody2D rb, float force);
    /// <summary>True if dashing is enabled for the current form.</summary>
    bool CanDash { get; }
    /// <summary>Perform a dash. Returns the dash end position so the caller can lerp.</summary>
    Vector2 Dash(Rigidbody2D rb, float direction, float distance, float duration);
}
}