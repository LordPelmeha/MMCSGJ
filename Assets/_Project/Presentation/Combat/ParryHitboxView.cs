#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using UnityEngine;
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
    private bool _isActive;
    /// <summary>
    /// Turn the hitbox on for the given duration, then turn it off.
    /// </summary>
    public void ActivateHitbox(float window)
    {
        _parryWindow = window;
        _isActive = true;
        if (_parryCollider != null)
            _parryCollider.enabled = true;
    }
}
}