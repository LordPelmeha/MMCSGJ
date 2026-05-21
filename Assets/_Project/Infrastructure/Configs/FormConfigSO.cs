#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for one player form (Head or Body).
/// </summary>
[CreateAssetMenu(menuName = "Configs/Form Config", fileName = "NewFormConfig")]
public class FormConfigSO : ScriptableObject
{
    [Header("Health")]
    [Tooltip("Maximum HP for this form.")]
    public int maxHP = 100;
    [Header("Movement")]
    [Tooltip("Horizontal movement speed (units / sec).")]
    public float moveSpeed = 2f;
    [Tooltip("Can this form jump?")]
    public bool canJump = false;
    [Tooltip("Upward impulse applied when jumping.")]
    public float jumpForce = 12f;
    [Tooltip("Can this form dash?")]
    public bool canDash = false;
    [Tooltip("Distance covered by one dash.")]
    public float dashDistance = 4f;
    [Tooltip("Time in seconds a dash takes.")]
    public float dashDuration = 0.15f;
    [Tooltip("Cooldown between dashes in seconds.")]
    public float dashCooldown = 1f;
    [Tooltip("Is the player invincible during a dash?")]
    public bool dashInvincible = true;
    [Header("Combat")]
    [Tooltip("Base damage per shot for this form.")]
    public float shootDamage = 25f;
    [Tooltip("Seconds between shots at full rate of fire.")]
    public float shootRate = 0.4f;
    [Tooltip("Speed of projectiles fired by this form.")]
    public float projectileSpeed = 15f;
    [Tooltip("Damage multiplier (Body form uses 0.5x).")]
    public float damageMultiplier = 1f;
    [Header("Marking (Head only)")]
    [Tooltip("How long a mark stays active (seconds).")]
    public float markDuration = 10f;
    [Tooltip("Maximum number of concurrent marks.")]
    public int maxMarks = 5;
    [Header("Vision")]
    [Tooltip("True = full vision. False = limited vision with Fog of War.")]
    public bool fullVision = true;
    [Tooltip("Inner clear radius of the vision area (world units).")]
    public float innerVisionRadius = 3f;
    [Tooltip("Outer dimmed radius beyond the inner zone (world units).")]
    public float outerVisionRadius = 5f;
    [Tooltip("Alpha multiplier in the outer dimmed zone (0 = fully transparent).")]
    public float outerVisionAlpha = 0.4f;
}
}