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
}
}