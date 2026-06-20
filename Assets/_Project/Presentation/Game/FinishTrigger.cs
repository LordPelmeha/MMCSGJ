  #nullable enable
  using HalfEmpty.Application.Game;
  using HalfEmpty.Presentation.UI;
  using UnityEngine;
  namespace HalfEmpty.Presentation.Game
  {
  /// <summary>
  /// Game finish trigger: when the player enters this zone, the game switches
  /// to CreditsState (shows "Спасибо, что играли").
  /// </summary>
  public class FinishTrigger : MonoBehaviour
  {
      [Header("Settings")]
      [SerializeField] private bool _debugLog = true;

      private bool _triggered;

      private void OnTriggerEnter2D(Collider2D other)
      {
          if (_triggered) return;
          if (!other.CompareTag("Player")) return;

          _triggered = true;
          if (_debugLog) Debug.Log("[FinishTrigger] Player reached the end — switching to CreditsState.");

          // Find GameFlowController in the scene and switch state
          var controller = Object.FindFirstObjectByType<HalfEmpty.Presentation.Game.GameFlowController>();
          if (controller != null)
          {
              controller.ChangeState(new CreditsState());
          }
          else
          {
              Debug.LogWarning("[FinishTrigger] GameFlowController not found in scene!");
          }
      }
  }
  }