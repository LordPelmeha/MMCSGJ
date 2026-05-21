#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy idle / patrol state. Waits for the player to be detected.
/// </summary>
public class EnemyIdleState : IState
{
    private readonly EnemyView _enemy;
    private readonly DetectionLogic _detection;
    private readonly float _patrolWaitTime;
    private float _timer;
    /// <summary>
    /// Creates the idle state.
    /// </summary>
    public EnemyIdleState(EnemyView enemy, DetectionLogic detection, float patrolWaitTime = 2f)
    {
        _enemy = enemy;
        _detection = detection;
        _patrolWaitTime = patrolWaitTime;
    }
    public void Enter() { }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}
}