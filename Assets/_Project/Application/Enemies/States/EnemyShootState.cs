 #nullable enable
 using HalfEmpty.Domain.Enums;
 using HalfEmpty.Domain.Health;
 using HalfEmpty.Infrastructure.Configs;
 using HalfEmpty.Infrastructure.Factories;
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
     private readonly ProjectileConfigSO? _projectileConfig;
     private readonly ProjectileFactory? _projectileFactory;
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
         ProjectileConfigSO? projectileConfig = null,
         ProjectileFactory? projectileFactory = null)
     {
         _enemy = enemy;
         _playerTransform = playerTransform;
         _fireRate = fireRate;
         _attackDamage = attackDamage;
         _projectileSpeed = projectileSpeed;
         _projectileConfig = projectileConfig;
         _projectileFactory = projectileFactory;
         _timer = 0f;
     }
     public void Enter()
     {
         _timer = _fireRate;
     }
     public void Exit() { }
     public void Update()
     {
         _timer -= Time.deltaTime;
         if (_timer <= 0f)
         {
             Fire();
             _timer = _fireRate;
         }
     }
     public void FixedUpdate() { }
      private void Fire()
      {
          if (_projectileFactory == null)
          {
              Debug.LogWarning("[EnemyShootState] _projectileFactory is null!");
              return;
          }
          if (_projectileConfig == null)
          {
              Debug.LogWarning("[EnemyShootState] _projectileConfig is null!");
              return;
          }
          if (_playerTransform == null)
          {
              Debug.LogWarning("[EnemyShootState] _playerTransform is null!");
              return;
          }
          Vector2 dir = (_playerTransform.position - _enemy.transform.position).normalized;
          // Try to use fire point from RangedEnemyView, fallback to enemy position
          Vector2 spawnPos = _enemy.transform.position;
          var rangedView = _enemy as HalfEmpty.Presentation.Enemies.RangedEnemyView;
          if (rangedView != null)
          {
              var firePoint = rangedView.GetFirePoint();
              if (firePoint != null)
              {
                  spawnPos = firePoint.position;
              }
          }
          Debug.Log($"[EnemyShootState] Firing projectile! pos={spawnPos} dir={dir} config={_projectileConfig.name}");
          _projectileFactory.Create(
              position: spawnPos,
              direction: dir,
              config: _projectileConfig,
              fromEnemy: true);
      }
  }
  }