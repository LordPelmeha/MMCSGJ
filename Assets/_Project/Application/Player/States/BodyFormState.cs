#nullable enable
using HalfEmpty.Domain.Enums;
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
    private readonly BodyMovementStrategy _strategy = new();
    /// <summary>
    /// Creates a BodyFormState that delegates back to the given controller.
    /// </summary>
    public BodyFormState(PlayerController controller)
    {
        _controller = controller;
    }
    public void Enter()
    {
        _controller.SetForm(FormType.Body);
        if (_controller.MovementView != null)
        {
            _controller.MovementView.SetStrategy(_strategy);
            var config = _controller.Config;
            if (config != null && config.bodyFormConfig != null)
                _controller.MovementView.SetSpeed(config.bodyFormConfig.moveSpeed);
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
