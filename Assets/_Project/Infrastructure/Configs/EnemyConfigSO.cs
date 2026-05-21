#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for enemies (shared by Melee and Ranged variants).
/// </summary>
[CreateAssetMenu(menuName = "Configs/Enemy Config", fileName = "NewEnemyConfig")]
public class EnemyConfigSO : ScriptableObject
{
    [Header("Health")]
    public int hp = 50;
    [Header("Movement")]
    public float moveSpeed = 4f;
    [Header("Detection")]
    [Tooltip("Maximum distance at which the player can be detected.")]
    public float detectionRange = 10f;
    [Tooltip("Field-of-view half-angle in degrees. Use 180 for 360°.")]
    public float detectionAngle = 180f;
    [Header("Attack")]
    public float attackDamage = 20f;
    [Tooltip("Distance at which the enemy will attack.")]
    public float attackRange = 1.5f;
    [Tooltip("Seconds between attacks.")]
    public float attackCooldown = 1f;
    [Header("Ranged (if applicable)")]
    public float projectileSpeed = 10f;
    public float fireRate = 1.5f;
    public GameObject? projectilePrefab;
    [Header("Behavior")]
    [Tooltip("Can this enemy be killed by a parry?")]
    public bool canBeParried = true;
}
}