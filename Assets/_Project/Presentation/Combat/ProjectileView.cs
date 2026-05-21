#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Domain.Enums;
using UnityEngine;
namespace HalfEmpty.Presentation.Combat
{
/// <summary>
/// View for a projectile. Handles movement, lifetime, and collision.
/// </summary>
public class ProjectileView : MonoBehaviour
{
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
        }    }
}
