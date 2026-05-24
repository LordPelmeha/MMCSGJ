 #nullable enable
 using HalfEmpty.Infrastructure.Configs;
 using HalfEmpty.Domain.Enums;
 using UnityEngine;
 namespace HalfEmpty.Infrastructure.Factories {
 /// <summary>
 /// Creates and configures enemy GameObjects from a prefab and an EnemyConfigSO.
 /// </summary>
 public class EnemyFactory
 {
     private readonly GameObject _prefab;
     /// <summary>
     /// Initialise with the enemy prefab.
     /// </summary>
     public EnemyFactory(GameObject prefab)
     {
         _prefab = prefab;
     }
     /// <summary>
     /// Spawn an enemy at the given position with the given config.
     /// </summary>
     public GameObject CreateEnemy(Vector3 position, EnemyConfigSO config, FormType formType = FormType.Body)
     {
         var enemy = Object.Instantiate(_prefab, position, Quaternion.identity);
         var view = enemy.GetComponent<Presentation.Player.PlayerController>();
         var enemyView = enemy.GetComponent<Presentation.EnemyView>();
         if (enemyView != null)
         {
             enemyView.Initialize(config, formType);
         }
         else
         {
             Debug.LogWarning("[EnemyFactory] EnemyView component not found on prefab.");
         }
         return enemy;
     }
 }
 }