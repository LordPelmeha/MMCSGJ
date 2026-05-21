#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy chase state: move towards the player.
/// </summary>
public class EnemyChaseState : IState
{
    private readonly EnemyView _enemy;
    private readonly Transform _playerTransform;
    private readonly float _moveSpeed;
    /// <summary>
    /// Creates the chase state.
    /// </summary>
    public EnemyChaseState(EnemyView enemy, Transform playerTransform, float moveSpeed)
    {
        _enemy = enemy;
        _playerTransform = playerTransform;
        _moveSpeed = moveSpeed;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}