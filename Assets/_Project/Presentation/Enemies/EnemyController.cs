#nullable enable
using System;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using UnityEngine;
using HalfEmpty.Presentation.Enemies;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Enemies;
namespace HalfEmpty.Presentation.Enemies {
/// <summary>
/// Controls enemy behavior via a state machine. Coordinates detection, health, and states.
/// </summary>
[RequireComponent(typeof(EnemyView))]
public class EnemyController : MonoBehaviour {
    [Header("Events")]
    [SerializeField] private VoidEventSO? _onPlayerDeath;
    [Header("Config")]
    [SerializeField] private EnemyConfigSO? _config;
    [Header("References")]
    [SerializeField] private Transform? _playerTarget;
    [SerializeField] private Transform? _firePoint;
    [Header("Debug")]
    [SerializeField] private bool _autoFindPlayer = true;
    private EnemyView _view = null!;
    private EnemyStateMachine? _fsm;
    private DetectionLogic? _detection;
    private float _attackCooldownTimer;
    private bool _isDead;
    private RangedEnemyView? _rangedView;
    private void Awake() {
        _view = GetComponent<EnemyView>();
        _rangedView = GetComponent<RangedEnemyView>();
    }
    private void Start() {
        if (_playerTarget == null && _autoFindPlayer) {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTarget = player.transform;
                Debug.Log($"[EnemyController] Auto-found player: {player.name}");
            }
            else
            {
                Debug.LogWarning($"[EnemyController] Player not found by tag! Ensure Player GameObject has Tag=Player.");
            }
        }
        if (_playerTarget != null)
        {
            Debug.Log($"[EnemyController] Player target assigned: {_playerTarget.name}, dist={Vector2.Distance(transform.position, _playerTarget.position):F1}");
        }
        if (_config != null) {
            Debug.Log($"[EnemyController] Initializing with config: {_config.name}, type={_config.enemyType}, layerMask={_config.playerLayerMask}");
            Initialize(_config, FormType.Body, _firePoint);
        }
    }
    private void OnDestroy() {
        _fsm?.Dispose();
    }
    /// <summary>
    /// Initialise the enemy controller with config and optional fire point for ranged enemies.
    /// </summary>
    public void Initialize(EnemyConfigSO config, FormType formType, Transform? firePoint = null) {
        _view.Initialize(config, formType);
        _view.OnDied += HandleDeath;
        SetupStateMachine(config, firePoint);
    }
    private void SetupStateMachine(EnemyConfigSO config, Transform? firePoint) {
        var pool = FindObjectOfType<HalfEmpty.Infrastructure.Pools.ProjectilePool>();
        var factory = pool != null ? pool.Factory : null;
        if (_rangedView != null && factory != null) {
            _rangedView.SetProjectileFactory(factory);
        }
        _detection = new DetectionLogic(transform, config.detectionRange, 180f, config.playerLayerMask);
        var idleState = new EnemyIdleState(_view, _detection, 2f);
        var chaseState = new EnemyChaseState(_view, _playerTarget, config.moveSpeed, config.attackRange);
        EnemyAttackState? attackState;
        EnemyShootState? shootState;
        if (config.enemyType == EnemyType.Melee) {
            attackState = new EnemyAttackState(_view, _playerTarget, config.attackDamage, config.attackRange, config.attackCooldown);
            shootState = null;
        } else {
            attackState = null;
            shootState = new EnemyShootState(_view, _playerTarget, config.fireRate, config.attackDamage, config.projectileSpeed, config.projectileConfig, factory);
        }
        var deathState = new EnemyDeathState(_view, 2f);
        _fsm = new EnemyStateMachine(idleState, chaseState, attackState, shootState, deathState);
    }
    private void Update() {
        if (_isDead || _fsm == null) return;
        _detection?.UpdateDetection(_playerTarget);
        _fsm.Update();
        _attackCooldownTimer -= Time.deltaTime;
    }
    private void FixedUpdate() {
        _fsm?.FixedUpdate();
    }
    private void HandleDeath() {
        _isDead = true;
        _fsm?.TransitionToDeath();
    }
}
}
/// <summary>
/// Enemy state machine orchestrator that handles state transitions.
/// </summary>
public class EnemyStateMachine : IDisposable {
    private readonly EnemyIdleState _idle;
    private readonly EnemyChaseState _chase;
    private readonly EnemyAttackState? _attack;
    private readonly EnemyShootState? _shoot;
    private readonly EnemyDeathState _death;
    private IState _current;
    public EnemyStateMachine(EnemyIdleState idle, EnemyChaseState chase, EnemyAttackState? attack, EnemyShootState? shoot, EnemyDeathState death) {
        _idle = idle;
        _chase = chase;
        _attack = attack;
        _shoot = shoot;
        _death = death;
        _current = idle;
    }
    public void Update() {
        _current.Update();
        Transition();
    }
    public void FixedUpdate() => _current.FixedUpdate();
    private void Transition() {
        if (_current == _death) return;
        if (_current == _idle && _idle.WasPlayerDetected()) {
            ChangeState(_chase);
        } else if (_current == _chase) {
            if (_shoot != null && _chase.IsInAttackRange()) {
                ChangeState(_shoot);
            } else if (_attack != null && _chase.IsInAttackRange()) {
                ChangeState(_attack);
            }
        } else if (_current == _attack && _attack.IsCooldownExpired) {
            ChangeState(_chase);
        } else if (_current == _shoot && _chase.IsInAttackRange() == false) {
            ChangeState(_chase);
        }
    }
    private void ChangeState(IState next) {
        _current.Exit();
        _current = next;
        _current.Enter();
    }
    public void TransitionToDeath() => ChangeState(_death);
    public void Dispose() {
        _idle.Exit();
        _chase.Exit();
        _attack?.Exit();
        _shoot?.Exit();
        _death.Exit();
    }
}