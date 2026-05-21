#nullable enable
using System;
using HalfEmpty.Domain.Health;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using UnityEngine;
namespace HalfEmpty.Presentation.Player
{
/// <summary>
/// Manages the active form's HP independently. Subscribes to damage events and raises OnHealthChanged.
/// </summary>
public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private PlayerConfigSO? _config;
    private HealthData? _headHealth;
    private HealthData? _bodyHealth;
    /// <summary>Fires with (currentHP, maxHP) whenever HP changes.</summary>
    public event Action<float, float>? OnHealthChanged;
    /// <summary>Fires when either form dies.</summary>
    public event Action? OnDeath;
    private void Awake()
    {
        if (_config == null)
        {
            Debug.LogError("PlayerHealthView: PlayerConfig is missing.");
            return;
        }
    }
}
}
