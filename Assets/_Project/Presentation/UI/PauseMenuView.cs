#nullable enable
using InputSystem_ActionsNamespace;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace HalfEmpty.Presentation.UI
{
/// <summary>
/// Pause menu with Resume, Restart, and Quit buttons.
/// </summary>
public class PauseMenuView : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private InputSystem_Actions? _inputActions;
    private bool _isPaused;
    private void Update()
    {
        if (_inputActions != null)
        {
            if (_inputActions.Player.Pause.triggered && _isPaused)
                ResumeGame();
            else if (_inputActions.Player.Pause.triggered && !_isPaused)
                PauseGame();
        }
    }
    private void PauseGame() { }
    private void ResumeGame() { }
}
}
