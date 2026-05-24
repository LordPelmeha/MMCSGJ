#nullable enable
using HalfEmpty.Application.FSM;
using HalfEmpty.Application;
using UnityEngine;
using HalfEmpty.Presentation.UI;
namespace HalfEmpty.Application.Game
{
/// <summary>
/// Paused game state.
/// </summary>
public class PausedState : IState
{
    public void Enter()
    {
        Time.timeScale = 0f;
        var pauseMenu = Object.FindFirstObjectByType<PauseMenuView>();
        if (pauseMenu != null) pauseMenu.gameObject.SetActive(true);
        else Debug.LogWarning("[PausedState] PauseMenuView not found in scene.");
    }
    public void Update()
    {
    }
    public void FixedUpdate() { }
    public void Exit()
    {
        Time.timeScale = 1f;
        var pauseMenu = Object.FindFirstObjectByType<PauseMenuView>();
        if (pauseMenu != null) pauseMenu.gameObject.SetActive(false);
    }
}
}
