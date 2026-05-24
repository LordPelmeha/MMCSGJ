 #nullable enable
 using HalfEmpty.Domain.Enums;
 using HalfEmpty.Infrastructure.Configs;
 using HalfEmpty.Infrastructure.Pools;
 using UnityEngine;
 namespace HalfEmpty.Infrastructure.Factories {
 /// <summary>
 /// Creates projectile instances using a pool under the hood.
 /// Supports player and enemy projectiles via the ProjectileConfigSO.
 /// </summary>
 public class ProjectileFactory
 {
     private readonly ProjectilePool _pool;
     /// <summary>
     /// Initialise with an existing pool.
     /// </summary>
     public ProjectileFactory(ProjectilePool pool)
     {
         _pool = pool;
     }
/// <summary>
/// Spawn a projectile from the pool at the given position and direction.
/// </summary>
public Presentation.Combat.ProjectileView Create(Vector2 position, Vector2 direction, ProjectileConfigSO config, bool fromEnemy = false)
{
    var proj = _pool.Get();
    if (proj == null) return null;
    proj.transform.position = position;
    proj.gameObject.SetActive(true);
    proj.Initialise(
        damage: config.damage,
        speed: config.speed,
        direction: direction,
        targetLayer: config.targetLayer,
        pool: _pool,
        canBeParried: config.canBeParried,
        reflectedSpeedMultiplier: config.reflectedSpeedMultiplier,
        fromEnemy: fromEnemy,
        lifetime: config.lifetime);
    return proj;
}
 }
 }
