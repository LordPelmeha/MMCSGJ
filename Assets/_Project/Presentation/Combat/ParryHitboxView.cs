 #nullable enable
 using HalfEmpty.Infrastructure.Configs;
 using HalfEmpty.Infrastructure.Events;
 using UnityEngine;
 using System.Collections;
 using HalfEmpty.Domain.Enums;
 namespace HalfEmpty.Presentation.Combat {
 /// <summary>
 /// Temporarily activates a trigger collider in front of the player to intercept melee attacks
 /// and enemy projectiles for a parry window.
 /// </summary>
 public class ParryHitboxView : MonoBehaviour
 {
     [Header("Parry Settings")]
     [SerializeField] private float _parryWindow = 0.3f;
     [SerializeField] private Collider2D? _parryCollider;
     [SerializeField] private PlayerConfigSO? _playerConfig;
     private Coroutine? _activeWindowCoroutine;
     private bool _isActive;
     /// <summary>
     /// Turn the hitbox on for the given duration, then turn it off.
     /// </summary>
     public void ActivateHitbox(float window)
     {
         if (_activeWindowCoroutine != null)
         {
             StopCoroutine(_activeWindowCoroutine);
             _activeWindowCoroutine = null;
         }
         _parryWindow = window;
         _isActive = true;
         if (_parryCollider != null)
             _parryCollider.enabled = true;
         _activeWindowCoroutine = StartCoroutine(HitboxWindowRoutine(window));
     }
     /// <summary>
     /// Manually deactivate the parry hitbox immediately.
     /// </summary>
     public void DeactivateHitbox()
     {
         if (_activeWindowCoroutine != null)
         {
             StopCoroutine(_activeWindowCoroutine);
             _activeWindowCoroutine = null;
         }
         _isActive = false;
         if (_parryCollider != null)
             _parryCollider.enabled = false;
     }
private IEnumerator HitboxWindowRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isActive = false;
        if (_parryCollider != null)
            _parryCollider.enabled = false;
        _activeWindowCoroutine = null;
    }
    /// <summary>True while the parry window is active.</summary>
    public bool IsActive => _isActive;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;
        // Check for projectiles to parry
        var projectile = other.GetComponent<HalfEmpty.Presentation.Combat.ProjectileView>();
        if (projectile != null)
        {
            projectile.OnParried();
            DeactivateHitbox();
        }
    }
}
 }