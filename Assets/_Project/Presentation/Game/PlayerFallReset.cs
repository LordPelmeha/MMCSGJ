#nullable enable
using UnityEngine;
using HalfEmpty.Presentation.Player;
using HalfEmpty.Infrastructure.Events;

namespace HalfEmpty.Presentation.Game
{
    /// <summary>
    /// Reset the game if player falls too low (off platform).
    /// </summary>
    public class PlayerFallReset : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fallThreshold = -10f;
        [SerializeField] private float _fallCheckDelay = 0.5f;
        [SerializeField] private GameObject? _player;
        [SerializeField] private Transform? _playerTransform;
        [SerializeField] private PlayerController? _playerController;
        [SerializeField] private VoidEventSO? _onPlayerDeath;
        [SerializeField] private VoidEventSO? _onGameReset;
        
        private bool _hasFallen = false;
        private float _lastPlayerY = 0f;
        
        private void Start()
        {
            // Find player if not assigned
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
                if (_player == null)
                {
                    Debug.LogError("[PlayerFallReset] Player not found! Please assign player object.");
                    enabled = false;
                    return;
                }
            }
            
            _playerTransform = _player.transform;
            _playerController = _player.GetComponent<PlayerController>();
            _lastPlayerY = _playerTransform.position.y;
        }
        
        private void Update()
        {
            if (_playerTransform == null || _hasFallen) return;
            
            float currentY = _playerTransform.position.y;
            
            // Check if player is falling
            if (currentY < _lastPlayerY)
            {
                // Check if fallen below threshold
                if (currentY <= _fallThreshold)
                {
                    ResetGame();
                    return;
                }
            }
            
            _lastPlayerY = currentY;
        }
        
        private void ResetGame()
        {
            if (_hasFallen) return;
            
            _hasFallen = true;
            Debug.Log($"[PlayerFallReset] Player fell below {_fallThreshold}. Resetting game...");
            
            // Trigger player death event if available
            _onPlayerDeath?.Raise();
            
            // Delay before actual reset
            Invoke(nameof(ActuallyResetGame), _fallCheckDelay);
        }
        
        private void ActuallyResetGame()
        {
            Debug.Log("[PlayerFallReset] Actually resetting game...");
            
            // Trigger game reset event
            _onGameReset?.Raise();
            
            // Alternative: Restart the scene
            // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            
            // Or reload the current level
            // Application.LoadLevel(Application.loadedLevel);
        }
        
        // Draw debug gizmo for fall threshold
        private void OnDrawGizmosSelected()
        {
            if (_playerTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    new Vector3(_playerTransform.position.x - 5f, _fallThreshold, 0),
                    new Vector3(_playerTransform.position.x + 5f, _fallThreshold, 0)
                );
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(
                    new Vector3(_playerTransform.position.x, _fallThreshold, 0),
                    0.5f
                );
            }
        }
        
        // Reset state when player respawns
        public void OnPlayerRespawn()
        {
            _hasFallen = false;
            _lastPlayerY = _playerTransform != null ? _playerTransform.position.y : 0f;
            Debug.Log("[PlayerFallReset] Player respawned. Reset fall detection.");
        }
    }
}