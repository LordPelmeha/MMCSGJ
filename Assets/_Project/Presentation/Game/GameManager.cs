#nullable enable
using HalfEmpty.Infrastructure.Events;
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
}
}
