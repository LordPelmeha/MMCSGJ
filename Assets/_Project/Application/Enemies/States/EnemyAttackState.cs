 #nullable enable
 using HalfEmpty.Domain.Enums;
 using HalfEmpty.Domain.Health;
 using HalfEmpty.Domain.Combat;
 using UnityEngine;
 using HalfEmpty.Presentation;
 using HalfEmpty.Application.Enemies.States;
 using HalfEmpty.Application.FSM;
 namespace HalfEmpty.Application.Enemies.States {
 /// <summary>
 /// Enemy melee attack state. Applies damage to the player, then returns to chase.
 /// </summary>
 public class EnemyAttackState : IState
 {
     private readonly EnemyView _enemy;
     private readonly Transform _playerTransform;
     private readonly float _attackDamage;
     private readonly float _attackRange;
     private readonly float _attackCooldown;
     private float _cooldownTimer;
     private bool _hasAttacked;
     /// <summary>
     /// Creates the attack state.
     /// </summary>
     public EnemyAttackState(
         EnemyView enemy,
         Transform playerTransform,
         float attackDamage,
         float attackRange,
         float attackCooldown)
     {
         _enemy = enemy;
         _playerTransform = playerTransform;
         _attackDamage = attackDamage;
         _attackRange = attackRange;
         _attackCooldown = attackCooldown;
         _cooldownTimer = attackCooldown;
         _hasAttacked = false;
     }
     public void Enter()
     {
         _cooldownTimer = _attackCooldown;
         _hasAttacked = false;
     }
     public void Exit() { }
     public void Update()
     {
         _cooldownTimer -= Time.deltaTime;
         if (_hasAttacked && _cooldownTimer <= 0f)
         {
             // Cooldown expired — handled by transition to ChaseState externally
         }
     }
     public void FixedUpdate()
     {
         float distance = Vector2.Distance(_enemy.transform.position, _playerTransform.position);
         if (distance <= _attackRange && !_hasAttacked)
         {
             // Deal direct melee damage to player
             var playerCtrl = _playerTransform.GetComponent<HalfEmpty.Presentation.Player.PlayerController>();
             if (playerCtrl != null)
             {
                 var healthView = playerCtrl.GetComponent<HalfEmpty.Presentation.Player.PlayerHealthView>();
                 healthView?.TakeDamage(HalfEmpty.Domain.Enums.FormType.Body, _attackDamage);
             }
             _hasAttacked = true;
             _cooldownTimer = _attackCooldown;
         }
     }
 }
 }