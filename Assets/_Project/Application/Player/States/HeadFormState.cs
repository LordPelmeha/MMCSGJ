#nullable enable
using HalfEmpty.Domain.Enums;
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
    private readonly HeadMovementStrategy _strategy = new();
    /// <summary>
    /// Creates a HeadFormState that delegates back to the given controller.
    /// </summary>
    public HeadFormState(PlayerController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        _controller.SetForm(FormType.Head);
        if (_controller.MovementView != null)
        {
            _controller.MovementView.SetStrategy(_strategy);
            var config = _controller.Config;
            if (config != null && config.headFormConfig != null)
                _controller.MovementView.SetSpeed(config.headFormConfig.moveSpeed);
        }
    }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate()
    {
        if (_controller.InputProvider != null && _controller.MovementView != null)
            _controller.MovementView.FixedUpdate(_controller.InputProvider.HorizontalAxis);
    }
}
}
