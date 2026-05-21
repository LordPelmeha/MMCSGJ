#nullable enable
using UnityEngine;
namespace HalfEmpty.Domain.Health {
/// <summary>
/// Legacy bridge: MonoBehaviour wrapper that owns a HealthData instance.
/// Kept for compatibility with the DisDoc plan. Prefer using HealthData directly.
/// </summary>
public class Life : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHP = 100f;
    public HealthData Data { get; private set; }
    private void Awake()
    {
        Data = new HealthData(_maxHP);
    }
}
}