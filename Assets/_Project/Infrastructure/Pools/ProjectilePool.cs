#nullable enable
using UnityEngine;
using System.Collections.Generic;
using HalfEmpty.Presentation.Combat;
using HalfEmpty.Infrastructure.Factories;
namespace HalfEmpty.Infrastructure.Pools
{
/// <summary>
/// Generic Object Pool for MonoBehaviour instances (e.g. ProjectileView).
/// Also provides a ProjectileFactory for easy spawning.
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private Presentation.Combat.ProjectileView? _prefab;
    [SerializeField] private int _initialSize = 20;
    private readonly Queue<ProjectileView> _pool = new();
    private ProjectileFactory? _factory;
    /// <summary>Get the factory for spawning projectiles.</summary>
    public ProjectileFactory Factory => _factory ??= new ProjectileFactory(this);
    private void Start()
    {
        InitPool();
    }
    /// <summary>Pre-warm the pool with inactive instances.</summary>
    private void InitPool()
    {
        if (_prefab == null) return;
        for (int i = 0; i < _initialSize; i++)
        {
            var obj = Object.Instantiate(_prefab, transform);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    /// <summary>Get or create from pool.</summary>
    public ProjectileView Get()
    {
        if (_pool.Count > 0)
        {
            var obj = _pool.Dequeue();
            if (obj != null) return obj;
        }
        if (_prefab != null)
        {
            var obj = Object.Instantiate(_prefab, transform);
            return obj;
        }
        return null;
    }
    /// <summary>Return a projectile to the pool for reuse.</summary>
    public void Return(ProjectileView obj)
    {
        if (obj == null) return;
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
}
