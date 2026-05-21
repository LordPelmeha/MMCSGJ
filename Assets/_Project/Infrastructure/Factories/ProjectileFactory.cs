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
}
}
