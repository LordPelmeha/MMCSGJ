#nullable enable
using HalfEmpty.Application.Player;
using UnityEngine;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// View that delegates movement to the current IMovementStrategy.
/// </summary>
public class PlayerMovementView
{
    private IMovementStrategy _strategy = null!;
    private Rigidbody2D? _rb;
    private float _speed;
    private bool _isGrounded;
    /// <summary>
    /// Set the active movement strategy (swap on form change).
    /// </summary>
    public void SetStrategy(IMovementStrategy strategy)
    {
        _strategy = strategy;
    }
}
}