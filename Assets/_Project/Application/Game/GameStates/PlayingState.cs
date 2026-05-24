#nullable enable
using HalfEmpty.Application.FSM;
using HalfEmpty.Application;
using UnityEngine;
using UnityEngine.InputSystem;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Presentation;
using HalfEmpty.Presentation.Game;
using HalfEmpty.Presentation.UI;
namespace HalfEmpty.Application.Game
{
/// <summary>
/// Active game-play state.
/// </summary>
public class PlayingState : IState
{
    private VoidEventSO? _onPlayerDeath;
    private GameFlowSM? _gameFlowSM;
    public void Enter()
    {
        Time.timeScale = 1f;
        // Find the GameFlowController in the scene, then get its GameFlowSM reference
        var controller = Object.FindFirstObjectByType<GameFlowController>();
        if (controller != null)
        {
            _gameFlowSM = controller.GameFlowSMRef;
        }
        _onPlayerDeath = Resources.Load<VoidEventSO>("Configs/Events/OnPlayerDeath");
        if (_onPlayerDeath != null)
        {
            _onPlayerDeath.Register(HandlePlayerDeath);
        }
        // Show HUD if present
        var hud = Object.FindFirstObjectByType<HUDView>();
        if (hud != null) hud.gameObject.SetActive(true);
    }
    public void Update()
    {
        // Check for pause input
        if (Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
        {
            _gameFlowSM?.ChangeState(new PausedState());
        }
    }
    public void FixedUpdate() { }
    public void Exit()
    {
        Time.timeScale = 1f;
        _onPlayerDeath?.Unregister(HandlePlayerDeath);
        var hud = Object.FindFirstObjectByType<HUDView>();
        if (hud != null) hud.gameObject.SetActive(false);
    }
    private void HandlePlayerDeath()
    {
        _gameFlowSM?.ChangeState(new GameOverState());
    }
}
}
