#nullable enable
using UnityEngine;
using HalfEmpty.Application.FSM;
using HalfEmpty.Presentation.Player;
namespace HalfEmpty.Application.Player {
/// <summary>
/// State for the Body form: fast movement, limited vision, parry & dash enabled.
/// </summary>
public class BodyFormState : IState
{
    private readonly PlayerController _controller;
    /// <summary>
    /// Creates a BodyFormState that delegates back to the given controller.
    /// </summary>
    public BodyFormState(PlayerController controller)
    {
        _controller = controller;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}