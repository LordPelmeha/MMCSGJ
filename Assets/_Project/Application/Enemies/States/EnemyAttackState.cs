#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy melee attack state. Applies damage to the player, then returns to chase.
/// </summary>
public class EnemyAttackState : IState
{
    private readonly EnemyView _enemy;
    private readonly Transform _playerTransform;
    private readonly float _attackDamage;
    private readonly float _attackRange;
    private readonly float _attackCooldown;
    private float _cooldownTimer;
    private bool _hasAttacked;
    /// <summary>
    /// Creates the attack state.
    /// </summary>
    public EnemyAttackState(
        EnemyView enemy,
        Transform playerTransform,
        float attackDamage,
        float attackRange,
        float attackCooldown)
    {
        _enemy = enemy;
        _playerTransform = playerTransform;
        _attackDamage = attackDamage;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _cooldownTimer = attackCooldown;
        _hasAttacked = false;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}