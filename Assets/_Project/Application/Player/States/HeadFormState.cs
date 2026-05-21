#nullable enable
using UnityEngine;
using HalfEmpty.Application.FSM;
using HalfEmpty.Presentation.Player;
namespace HalfEmpty.Application.Player {
/// <summary>
/// State for the Head form: slow movement, full vision, marking enabled.
/// </summary>
public class HeadFormState : IState
{
    private readonly PlayerController _controller;
    /// <summary>
    /// Creates a HeadFormState that delegates back to the given controller.
    /// </summary>
    public HeadFormState(PlayerController controller)
    {
        _controller = controller;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}