#nullable enable
using System;
namespace HalfEmpty.Domain.Health {
/// <summary>
/// Pure C# health data. No MonoBehaviour dependency.
/// </summary>
public class HealthData
{
    public float CurrentHP { get; private set; }
    public float MaxHP { get; }
    public bool IsDead => CurrentHP <= 0f;
    /// <summary>Fires when HP changes. (current, max)</summary>
    public event Action<float, float>? OnHealthChanged;
    /// <summary>Fires when health reaches zero.</summary>
    public event Action? OnDied;
    /// <summary>
    /// Initialise with the maximum hit points.
    /// </summary>
    public HealthData(float maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }
}
}