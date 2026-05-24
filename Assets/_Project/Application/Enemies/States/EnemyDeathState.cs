#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
using System.Collections;
namespace HalfEmpty.Application.Enemies.States {
/// <summary>
/// Enemy death state: plays death animation, disables colliders, destroys after a delay.
/// </summary>
public class EnemyDeathState : IState
{
    private readonly EnemyView _enemy;
    private readonly float _destroyDelay;
    private bool _entered;
    /// <summary>
    /// Creates the death state.
    /// </summary>
    public EnemyDeathState(EnemyView enemy, float destroyDelay = 2f)
    {
        _enemy = enemy;
        _destroyDelay = destroyDelay;
    }
    public void Enter()
    {
        if (_entered) return;
        _entered = true;
        _enemy.OnDied += HandleDied;
        // Fire the event immediately (or let the caller have already fired it)
        HandleDied();
    }
    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
    private void HandleDied()
    {
        // Disable all colliders on enemy
        foreach (var col in _enemy.GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
        // Schedule destruction
        _enemy.StartCoroutine(DestroyAfterDelayRoutine());
    }
    private IEnumerator DestroyAfterDelayRoutine()
    {
        yield return new WaitForSeconds(_destroyDelay);
        Object.Destroy(_enemy.gameObject);
    }
}
}
