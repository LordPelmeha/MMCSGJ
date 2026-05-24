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
     private FormType _currentForm = FormType.Body;
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
         if (_config.headFormConfig != null)
         {
             _headHealth = new HealthData(_config.headFormConfig.maxHP);
             _headHealth.OnHealthChanged += (cur, max) => OnHealthChanged?.Invoke(cur, max);
             _headHealth.OnDied += HandleFormDeath;
         }
         if (_config.bodyFormConfig != null)
         {
             _bodyHealth = new HealthData(_config.bodyFormConfig.maxHP);
             _bodyHealth.OnHealthChanged += (cur, max) => OnHealthChanged?.Invoke(cur, max);
             _bodyHealth.OnDied += HandleFormDeath;
         }
     }
     private void HandleFormDeath()
     {
         OnDeath?.Invoke();
     }
     /// <summary>Switch the active form (called on form switch).</summary>
     public void SetForm(FormType form)
     {
         _currentForm = form;
     }
     /// <summary>Apply damage to the given form. Returns true if the form died.</summary>
     public bool TakeDamage(FormType form, float damage)
     {
         var health = form == FormType.Head ? _headHealth : _bodyHealth;
         if (health == null) return false;
         health.TakeDamage(damage);
         return health.IsDead;
     }
     /// <summary>Get current HP for the given form.</summary>
     public float GetCurrentHP(FormType form)
     {
         var health = form == FormType.Head ? _headHealth : _bodyHealth;
         return health != null ? health.CurrentHP : 0f;
     }
     /// <summary>Get max HP for the given form.</summary>
     public float GetMaxHP(FormType form)
     {
         var health = form == FormType.Head ? _headHealth : _bodyHealth;
         return health != null ? health.MaxHP : 0f;
     }
 }
 }
