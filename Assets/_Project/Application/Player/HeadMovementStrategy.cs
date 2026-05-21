#nullable enable
using UnityEngine;
namespace HalfEmpty.Application.Player {
/// <summary>
/// Slow, ground-only movement for the Head form. No jump, no dash.
/// </summary>
public class HeadMovementStrategy : IMovementStrategy
{
    public bool CanJump => false;
    public bool CanDash => false;
    public void Move(Rigidbody2D rb, float direction, float speed)
    {
        if (rb == null) return;
        var v = rb.linearVelocity;
        v.x = direction * speed;
        rb.linearVelocity = v;
    }
    public void Jump(Rigidbody2D rb, float force) { }
    public Vector2 Dash(Rigidbody2D rb, float direction, float distance, float duration) => rb.transform.position;
}
}