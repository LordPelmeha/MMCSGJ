#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Infrastructure.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
using HalfEmpty.Presentation.Game;
using UnityEngine.UI;
using TMPro;
namespace HalfEmpty.Presentation.UI
{
/// <summary>
/// Pause menu with Resume, Restart, and Quit buttons.
/// Doubles as the Game Over screen (Restart / Main Menu only).
/// </summary>
public class PauseMenuView : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private IInputProvider? _inputProvider;
    [Header("UI References")]
    [SerializeField] private GameObject? _resumeButton;
    [SerializeField] private GameObject? _restartButton;
    [SerializeField] private GameObject? _quitButton;
    [SerializeField] private TextMeshProUGUI? _titleText;
    [SerializeField] private string _pauseTitle = "PAUSED";
    [SerializeField] private string _gameOverTitle = "GAME OVER";
    private bool _isPaused;
    private bool _isGameOver;
    private void Start()
    {
        Hide();
    }
    private void Update()
    {
        if (_inputProvider != null && _inputProvider.PausePressed && !_isGameOver)
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    /// <summary>Show as Pause menu (Resume + Restart + Quit).</summary>
    public void PauseGame()
    {
        _isPaused = true;
        _isGameOver = false;
        Time.timeScale = 0f;
        Show(_pauseTitle);
    }
    /// <summary>Hide the menu and resume time.</summary>
    public void ResumeGame()
    {
        _isPaused = false;
        _isGameOver = false;
        Time.timeScale = 1f;
        Hide();
    }
    /// <summary>Show as Game Over screen (Restart + Main Menu only).</summary>
    public void ShowGameOver()
    {
        _isPaused = false;
        _isGameOver = true;
        Time.timeScale = 0f;
        Show(_gameOverTitle);
    }
    /// <summary>Called by the Restart button (works in both Pause and Game Over modes).</summary>
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        Hide();
        var gm = FindFirstObjectByType<HalfEmpty.Presentation.GameManager>();
        gm?.RestartGame();
    }
    /// <summary>Called by the Quit / Main Menu button.</summary>
    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        Hide();
        var gm = FindFirstObjectByType<HalfEmpty.Presentation.GameManager>();
        gm?.QuitToMenu();
    }
    private void Show(string title)
    {
        gameObject.SetActive(true);
        if (_titleText != null) _titleText.text = title;
        SetButtonsActive(_resumeButton, !_isGameOver);
        SetButtonsActive(_restartButton, true);
        SetButtonsActive(_quitButton, true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        _isPaused = false;
        _isGameOver = false;
    }
    private static void SetButtonsActive(GameObject? go, bool active)
    {
        if (go != null) go.SetActive(active);
    }
}
}