#nullable enable
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Application;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
using HalfEmpty.Presentation.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace HalfEmpty.Presentation
{
/// <summary>
/// Singleton Game Manager. Owns the global game flow and subscribes to high-level events.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Events")]
    [SerializeField] private VoidEventSO? _onPlayerDeath;
    [Header("Game Flow")]
    [SerializeField] private GameFlowSM? _gameFlowSM;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (_onPlayerDeath != null)
        {
            _onPlayerDeath.Register(OnPlayerDeath);
        }
        else
        {
            Debug.LogWarning("[GameManager] _onPlayerDeath event not assigned.");
        }

        if (_gameFlowSM == null)
        {
            var controller = FindFirstObjectByType<HalfEmpty.Presentation.Game.GameFlowController>();
            if (controller != null)
                _gameFlowSM = controller.GameFlowSMRef;
        }
    }
    private void OnDestroy()
    {
        if (_onPlayerDeath != null)
        {
            _onPlayerDeath.Unregister(OnPlayerDeath);
        }
    }
    private void OnPlayerDeath()
    {
        if (_gameFlowSM != null)
        {
            _gameFlowSM.ChangeState(new GameOverState());
        }
        else
        {
            Debug.LogWarning("[GameManager] _gameFlowSM not assigned — cannot go to Game Over state.");
        }
    }
    /// <summary>Restart the current scene.</summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>Quit to main menu.</summary>
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}
}
