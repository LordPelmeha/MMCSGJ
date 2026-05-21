#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy ranged shoot state. Fires a projectile at the player with the given fire rate.
/// </summary>
public class EnemyShootState : IState
{
    private readonly EnemyView _enemy;
    private readonly Transform _playerTransform;
    private readonly float _fireRate;
    private readonly float _attackDamage;
    private readonly float _projectileSpeed;
    private readonly GameObject _projectilePrefab;
    private float _timer;
    /// <summary>
    /// Creates the shoot state.
    /// </summary>
    public EnemyShootState(
        EnemyView enemy,
        Transform playerTransform,
        float fireRate,
        float attackDamage,
        float projectileSpeed,
        GameObject? projectilePrefab = null)
    {
        _enemy = enemy;
        _playerTransform = playerTransform;
        _fireRate = fireRate;
        _attackDamage = attackDamage;
        _projectileSpeed = projectileSpeed;
        _projectilePrefab = projectilePrefab;
        _timer = 0f;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}