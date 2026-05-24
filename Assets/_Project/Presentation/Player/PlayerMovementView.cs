using HalfEmpty.Application.Player;
using UnityEngine;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// View that delegates movement to the current IMovementStrategy.
/// Called every FixedUpdate from HeadFormState/BodyFormState via PlayerController.
/// </summary>
public class PlayerMovementView
{
    private IMovementStrategy _strategy = null!;
    private Rigidbody2D? _rb;
    private float _speed;
    private bool _dashing;
    /// <summary>Inject Rigidbody2D and ground-check settings from PlayerController.</summary>
    public void Setup(Rigidbody2D rb, Transform? groundCheck, float groundCheckRadius, LayerMask groundLayer)
    {
        _rb = rb;
    }
    /// <summary>Swap the active movement strategy (called on form change).</summary>
    public void SetStrategy(IMovementStrategy strategy)
    {
        _strategy = strategy;
    }
    /// <summary>Set movement speed from form config.</summary>
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    /// <summary>Set dash active flag so strategy/skip logic knows we are dashing.</summary>
    public void SetDashing(bool dashing)
    {
        _dashing = dashing;
    }
    /// <summary>True while a dash movement is in progress.</summary>
    public bool IsDashing => _dashing;
    /// <summary>Current active strategy.</summary>
    public IMovementStrategy? GetStrategy() => _strategy;
    /// <summary>
    /// Apply movement. Called every FixedUpdate from BodyFormState.
    /// Skips X-axis movement while dashing so the dash is not overwritten.
    /// </summary>
    public void FixedUpdate(float horizontalInput)
    {
        if (_rb == null || _strategy == null) return;
        if (_dashing) return; // Dash handles X velocity independently
        _strategy.Move(_rb, horizontalInput, _speed);
    }
}

}
