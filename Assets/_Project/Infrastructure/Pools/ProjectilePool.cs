#nullable enable
using UnityEngine;
using System.Collections.Generic;
namespace HalfEmpty.Infrastructure.Pools
{
/// <summary>
/// Generic Object Pool for MonoBehaviour instances (e.g. ProjectileView).
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private Presentation.Combat.ProjectileView? _prefab;
    [SerializeField] private int _initialSize = 20;
    private readonly Queue<Presentation.Combat.ProjectileView> _pool = new();
    /// <summary>Get or create from pool.</summary>
    public Presentation.Combat.ProjectileView Get()
    {
        if (_pool.Count > 0)
        {
            var obj = _pool.Dequeue();
            if (obj != null) return obj;
        }
        return null;
    }
}
}
