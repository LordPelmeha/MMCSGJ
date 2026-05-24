 #nullable enable
using System;
using UnityEngine;

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
     /// <summary>
     /// Apply raw damage. Clamps to 0 and fires events.
     /// </summary>
     public void TakeDamage(float amount)
     {
         if (IsDead) return;
         CurrentHP = Mathf.Max(0f, CurrentHP - amount);
         OnHealthChanged?.Invoke(CurrentHP, MaxHP);
         if (CurrentHP <= 0f)
         {
             OnDied?.Invoke();
         }
     }
 }
 }