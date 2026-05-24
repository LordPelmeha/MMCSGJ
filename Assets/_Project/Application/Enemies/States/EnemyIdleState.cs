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
     private bool _playerDetected;
     /// <summary>
     /// Creates the idle state.
     /// </summary>
     public EnemyIdleState(EnemyView enemy, DetectionLogic detection, float patrolWaitTime = 2f)
     {
         _enemy = enemy;
         _detection = detection;
         _patrolWaitTime = patrolWaitTime;
         _detection.OnPlayerDetected += HandlePlayerDetected;
     }
     public void Enter()
     {
         _timer = 0f;
         _playerDetected = false;
     }
     public void Exit()
     {
         _detection.OnPlayerDetected -= HandlePlayerDetected;
     }
     public void Update()
     {
         if (_playerDetected) return;
         // Detection is driven externally via UpdateDetection call from the enemy controller
     }
     public void FixedUpdate() { }
     /// <summary>
     /// Forward player position to the detection component each frame.
     /// </summary>
     public void UpdateDetection(Transform playerTransform)
     {
         _detection.UpdateDetection(playerTransform);
     }
     private void HandlePlayerDetected(Transform player)
     {
         _playerDetected = true;
     }
 }
 }