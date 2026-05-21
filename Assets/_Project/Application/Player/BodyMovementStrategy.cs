#nullable enable
using UnityEngine;
namespace HalfEmpty.Application.Player {
/// <summary>
/// Fast movement with jump and dash for the Body form.
/// </summary>
public class BodyMovementStrategy : IMovementStrategy
{
    public bool CanJump => true;
    public bool CanDash => true;
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