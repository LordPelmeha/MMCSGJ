#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Domain.Enums;
using UnityEngine;
using System.Collections;
using HalfEmpty.Presentation.Player;
using HalfEmpty.Domain.Health;
using HalfEmpty.Domain.Combat;
namespace HalfEmpty.Presentation.Combat
{
/// <summary>
/// View for a projectile. Handles movement, lifetime, and collision.
/// </summary>
public class ProjectileView : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private bool _debugCollision = false;
    private Rigidbody2D? _rb;
    private Collider2D? _col;
    private SpriteRenderer? _sr;

    private float _damage;
    private float _speed;
    private Vector2 _direction;
    private LayerMask _targetLayer;
    private ProjectilePool? _pool;
    private bool _isReflected;
    private float _reflectedSpeedMultiplier = 1.5f;
    private float _lifetime = 5f;
    private bool _canBeParried = true;
    private bool _fromEnemy;

    /// <summary>
    /// Initialise the projectile with all required parameters.
    /// </summary>
    public void Initialise(
        float damage,
        float speed,
        Vector2 direction,
        LayerMask targetLayer,
        ProjectilePool pool,
        bool canBeParried = true,
        float reflectedSpeedMultiplier = 1.5f,
        bool fromEnemy = false,
        float lifetime = 5f)
    {
        _damage = damage;
        _speed = speed;
        _direction = direction.normalized;
        _targetLayer = targetLayer;
        _pool = pool;
        _canBeParried = canBeParried;
        _reflectedSpeedMultiplier = reflectedSpeedMultiplier;
        _fromEnemy = fromEnemy;
        _lifetime = lifetime;

        if (_rb != null)
        {
            _rb.linearVelocity = _direction * _speed;
            transform.rotation = Quaternion.AngleAxis(
                Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg, Vector3.forward);
        }

        // Start lifetime timer
        StopAllCoroutines();
        StartCoroutine(LifetimeRoutine());
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_pool == null) return;

// Check if hit the target layer
            if ((_targetLayer & (1 << other.gameObject.layer)) != 0)
            {
                // Deal damage via HealthData if present
                var enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null && enemyView.Health != null)
                {
                    enemyView.Health.TakeDamage(_damage);
                }
                else
                {
                    // Try PlayerHealthView for the player
                    var playerHealth = other.GetComponent<HalfEmpty.Presentation.Player.PlayerHealthView>();
                    if (playerHealth != null)
                    {
                        var hpHolder = other.GetComponent<HalfEmpty.Presentation.Player.PlayerController>();
                        if (hpHolder != null)
                        {
                            var form = hpHolder.CurrentForm;
                            playerHealth.TakeDamage(form, _damage);
                        }
                    }
                }

                if (_debugCollision) Debug.Log($"[ProjectileView] Hit {other.name} on target layer.");
                _pool.Return(this);
                return;
            }

        // Hit environment — return to pool
        if (other.CompareTag("Environment"))
        {
            if (_debugCollision) Debug.Log($"[ProjectileView] Hit environment: {other.name}");
            _pool.Return(this);
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        if (_pool != null)
        {
            _pool.Return(this);
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

/// <summary>Current target layer for collision detection.</summary>
    public LayerMask TargetLayer => _targetLayer;
    /// <summary>Called when the projectile is parried (reflected).</summary>
    public void OnParried()
    {
        if (!_canBeParried) return;
        _isReflected = true;
        _direction = -_direction;
        _speed *= _reflectedSpeedMultiplier;
        // Flip the target layer when parried - player projectiles target enemy, enemy projectiles target player
        _targetLayer = _fromEnemy ? LayerMask.GetMask("Player") : LayerMask.GetMask("Enemy");
        if (_targetLayer == 0)
            Debug.LogWarning($"[ProjectileView] Reflected projectile targetLayer is 0 — layer '{( _fromEnemy ? "Player" : "Enemy")}' may not exist.");
        if (_rb != null)
        {
            _rb.linearVelocity = _direction * _speed;
        }

        transform.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
}
