#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Events;
using UnityEngine;

namespace HalfEmpty.Presentation.Game
{
    /// <summary>
    /// Game finish trigger: Ends the game when player reaches designated trigger object.
    /// </summary>
    public class FinishTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private VoidEventSO _onGameFinish;
        [SerializeField] private bool _debugLog = true;
        
        private bool _triggerActivated = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && !_triggerActivated)
            {
                Debug.Log($"[FinishTrigger] Game finished via trigger: {gameObject.name}");
                _onGameFinish?.Raise();
                
                // Prevent multiple triggers from same object
                _triggerActivated = true;
            }
            else if (_debugLog)
            {
                Debug.Log($"[FinishTrigger] Ignoring non-player/trigger: {other.name}");
            }
        }
    }
}
