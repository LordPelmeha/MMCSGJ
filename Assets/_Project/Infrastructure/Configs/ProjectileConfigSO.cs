#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for projectiles (player and enemy).
/// </summary>
[CreateAssetMenu(menuName = "Configs/Projectile Config", fileName = "NewProjectileConfig")]
public class ProjectileConfigSO : ScriptableObject
{
    [Header("Damage & Speed")]
    public float damage = 15f;
    public float speed = 15f;
    [Header("Lifetime")]
    [Tooltip("Time in seconds before the projectile auto-destroys.")]
    public float lifetime = 5f;
    [Header("Collider Size")]
    public Vector2 colliderSize = new Vector2(0.3f, 0.3f);
    [Header("Sprite")]
    public Sprite? sprite;
    [Header("Parry")]
    public bool canBeParried = true;
    [Tooltip("Speed multiplier applied when the projectile is reflected.")]
    public float reflectedSpeedMultiplier = 1.5f;
    [Header("Layers")]
    [Tooltip("Which layers the projectile can damage.")]
    public LayerMask targetLayer;
    [Header("VFX")]
    [Tooltip("Optional particle effect for when the projectile is spawned.")]
    public GameObject? spawnVfx;
}
}