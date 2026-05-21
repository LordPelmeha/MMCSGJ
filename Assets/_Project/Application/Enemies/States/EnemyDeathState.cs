#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy death state: plays death animation, disables colliders, destroys after a delay.
/// </summary>
public class EnemyDeathState : IState
{
    private readonly EnemyView _enemy;
    private readonly float _destroyDelay;
    /// <summary>
    /// Creates the death state.
    /// </summary>
    public EnemyDeathState(EnemyView enemy, float destroyDelay = 2f)
    {
        _enemy = enemy;
        _destroyDelay = destroyDelay;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}