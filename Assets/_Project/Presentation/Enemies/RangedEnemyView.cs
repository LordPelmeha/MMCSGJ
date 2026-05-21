#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using UnityEngine;
namespace HalfEmpty.Presentation.Enemies {
/// <summary>
/// View for a ranged (turret) enemy. Delegates projectile creation to ProjectileFactory.
/// </summary>
public class RangedEnemyView : EnemyView
{
    [Header("Shooting")]
    [SerializeField] private Transform? _firePoint;
    [SerializeField] private float _fireRate = 1.5f;
    [SerializeField] private float _projectileSpeed = 10f;
    private ProjectileFactory? _projectileFactory;
    private float _fireTimer;
    /// <summary>Override with a pre-built factory (set during enemy spawn setup).</summary>
    public void SetProjectileFactory(ProjectileFactory factory)
    {
        _projectileFactory = factory;
    }
}
}