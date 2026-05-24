#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Infrastructure.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
using HalfEmpty.Presentation.Game;
namespace HalfEmpty.Presentation.UI
{
/// <summary>
/// Pause menu with Resume, Restart, and Quit buttons.
/// </summary>
public class PauseMenuView : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private IInputProvider? _inputProvider;
    private bool _isPaused;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        // Pause only from PlayingState
        if (_inputProvider != null && _inputProvider.PausePressed)
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    /// <summary>Called by the Resume button.</summary>
    public void OnResumeButton()
    {
        ResumeGame();
    }
    /// <summary>Called by the Restart button.</summary>
    public void OnRestartButton()
    {
        ResumeGame();
        var gm = FindObjectOfType<HalfEmpty.Presentation.GameManager>();
        gm?.RestartGame();
    }
    /// <summary>Called by the Quit to Menu button.</summary>
    public void OnQuitButton()
    {
        ResumeGame();
        var gm = FindObjectOfType<HalfEmpty.Presentation.GameManager>();
        gm?.QuitToMenu();
    }
    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
}