#nullable enable
using System;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using UnityEngine;
namespace HalfEmpty.Presentation
{
/// <summary>
/// Base View for all enemies. Wires health, state machine, and events.
/// </summary>
public class EnemyView : MonoBehaviour
{
    protected EnemyConfigSO? _config;
    protected HealthData? _healthData;
    protected Application.Enemies.States.EnemyDeathState? _deathState;
    private FormType _formType = FormType.Body;
    private bool _isDead;
    /// <summary>Raised when this enemy dies.</summary>
    public event Action? OnDied;
    /// <summary>
    /// Initialise the enemy with its config and form type.
    /// </summary>
    public void Initialize(EnemyConfigSO? config, FormType formType)
    {
        _config = config;
        _formType = formType;
        if (config != null)
        {
            _healthData = new HealthData(config.hp);
            _healthData.OnDied += HandleDeath;
            _healthData.OnHealthChanged += (cur, max) => { };
        }
    }
    /// <summary>
    /// Subscribed to the HealthData OnDied event.
    /// </summary>
    private void HandleDeath()
    {
        OnDied?.Invoke();
        _isDead = true;
    }
    /// <summary>
    /// Current health data for this enemy.
    /// </summary>
    public HealthData? Health => _healthData;
}
}
