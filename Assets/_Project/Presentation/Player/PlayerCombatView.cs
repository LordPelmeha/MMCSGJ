#nullable enable
using System;
using System.Collections;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Infrastructure.Input;
using HalfEmpty.Presentation.Combat;
using UnityEngine;
namespace HalfEmpty.Presentation.Player
{
/// <summary>
/// Handles shooting and parrying for the player.
/// </summary>
public class PlayerCombatView : MonoBehaviour
{
    private ProjectileFactory? _projectileFactory;
    private ProjectilePool? _currentPool;
    [Header("Combat References")]
    [SerializeField] private Transform? _firePoint;
    [SerializeField] private ParryHitboxView? _parryHitbox;
    [Header("Config")]
    [SerializeField] private FormConfigSO? _headFormConfig;
    [SerializeField] private FormConfigSO? _bodyFormConfig;
    [SerializeField] private PlayerConfigSO? _playerConfig;
    [SerializeField] private ProjectileConfigSO? _projectileConfig;
    [SerializeField] private FormType _currentForm;
    private float _shootCooldownTimer;
    private float _parryCooldownTimer;
    private bool _isParrying;
    private Coroutine? _parryCoroutine;
    /// <summary>Raised after a shot is fired.</summary>
    public event Action? OnShot;
    /// <summary>Raised after a successful parry.</summary>
    public event Action? OnParry;
    private void Start()
    {
        _shootCooldownTimer = 0f;
        _parryCooldownTimer = 0f;
    }
    private void Update()
    {
        // Reduce cooldown timers
        if (_shootCooldownTimer > 0f) _shootCooldownTimer -= Time.deltaTime;
        if (_parryCooldownTimer > 0f) _parryCooldownTimer -= Time.deltaTime;
    }
    /// <summary>Set the active form for damage/speed calculations.</summary>
    public void SetForm(FormType form)
    {
        _currentForm = form;
    }
    /// <summary>Call when a shoot input is detected.</summary>
    public void HandleShoot(IInputProvider input, FormConfigSO formConfig)
    {
        if (_projectileFactory == null)
        {
            Debug.LogWarning("[Combat] _projectileFactory is NULL!");
            return;
        }
        if (_shootCooldownTimer > 0f) 
        {
            Debug.Log($"[Combat] Shoot on cooldown: {_shootCooldownTimer:F2}");
            return;
        }
        if (_firePoint == null) 
        {
            Debug.LogWarning("[Combat] _firePoint is NULL!");
            return;
        }
        if (_projectileConfig == null)
        {
            Debug.LogWarning("[Combat] _projectileConfig is NULL!");
            return;
        }

        Vector2 direction = input.MouseWorldPosition - (Vector2)_firePoint.position;
        if (direction.sqrMagnitude < 0.01f) direction = _firePoint.right;

        Debug.Log($"[Combat] Shooting! dir={direction.normalized} muzzle={_firePoint.position}");
        _projectileFactory.Create(
            position: _firePoint.position,
            direction: direction.normalized,
            config: _projectileConfig,
            fromEnemy: false);

        _shootCooldownTimer = formConfig.shootRate;
        OnShot?.Invoke();
    }
    /// <summary>Call when a parry input is detected.</summary>
    public void HandleParry()
    {
        if (_parryCooldownTimer > 0f) return;
        if (_playerConfig == null) return;
        if (_parryHitbox == null) return;

        _parryHitbox.ActivateHitbox(_playerConfig.parryWindow);
        _parryCooldownTimer = _playerConfig.parryCooldown;
        OnParry?.Invoke();
    }
    /// <summary>Inject the projectile factory (set by spawn setup).</summary>
    public void SetProjectileFactory(ProjectileFactory factory)
    {
        _projectileFactory = factory;
    }
    /// <summary>Inject the projectile pool (set by spawn setup).</summary>
    public void SetProjectilePool(ProjectilePool pool)
    {
        _currentPool = pool;
     }
 }
 }

