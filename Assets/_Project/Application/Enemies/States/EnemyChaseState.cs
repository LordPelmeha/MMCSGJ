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
      private readonly float _attackRange;
      /// <summary>
      /// Creates the chase state.
      /// </summary>
      public EnemyChaseState(EnemyView enemy, Transform playerTransform, float moveSpeed, float attackRange = 1.5f)
      {
          _enemy = enemy;
          _playerTransform = playerTransform;
          _moveSpeed = moveSpeed;
          _attackRange = attackRange;
      }
      public void Enter() { }
      public void Exit() { }
      public void Update() { }
      public void FixedUpdate()
      {
          if (_playerTransform == null) return;
          Vector2 dir = (_playerTransform.position - _enemy.transform.position).normalized;
          var rb = _enemy.GetComponent<Rigidbody2D>();
          if (rb != null)
          {
              rb.linearVelocity = new Vector2(dir.x * _moveSpeed, rb.linearVelocity.y);
          }
      }
      /// <summary>True if the player is within attack range.</summary>
      public bool IsInAttackRange()
      {
          return _playerTransform != null
              && Vector2.Distance(_enemy.transform.position, _playerTransform.position) <= _attackRange;
      }
  }
  }
