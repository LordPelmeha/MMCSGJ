#nullable enable
using System.Collections.Generic;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Domain.Enums;
using UnityEngine;
namespace HalfEmpty.Presentation.Enemies {
/// <summary>
/// View for a melee enemy. Handles melee hitbox + close-range attack.
/// </summary>
public class MeleeEnemyView : EnemyView
{
    [Header("Melee")]
    [Tooltip("Optional transform where the melee hitbox appears during attack.")]
    [SerializeField] private Transform? _meleeHitboxOrigin;
    [SerializeField] private float _meleeHitboxRadius = 1f;
    [SerializeField] private LayerMask _playerLayer;
    private readonly List<Collider2D> _hitResults = new(4);
     /// <summary>
     /// Initialise the melee enemy.
     /// </summary>
     public new void Initialize(EnemyConfigSO config, FormType formType)
     {
         base.Initialize(config, formType);
     }
}
}