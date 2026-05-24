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
    public void Jump(Rigidbody2D rb, float force)
    {
        if (rb == null) return;
        var v = rb.linearVelocity;
        v.y = force;
        rb.linearVelocity = v;
        Debug.Log($"[BodyMovementStrategy] Jump! force={force:F1} rbVelY={v.y:F1}");
    }
    public Vector2 Dash(Rigidbody2D rb, float direction, float distance, float duration)
    {
        if (rb == null) return Vector2.zero;
        var startPos = (Vector2)rb.transform.position;
        var endPos = startPos + new Vector2(direction * distance, 0f);
        Debug.Log($"[BodyMovementStrategy] Dash start! dir={direction:F1} dist={distance:F1} dur={duration:F2} endPos={endPos}");
        return endPos;
    }
}

}
