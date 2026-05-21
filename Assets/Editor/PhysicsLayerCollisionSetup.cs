#nullable enable

using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor utility that configures the Physics2D layer collision matrix
/// according to DisDoc Appendix B when the project is loaded or recompiled.
/// Add this script to any Editor folder in the project.
/// </summary>
[InitializeOnLoad]
public static class PhysicsLayerCollisionSetup
{
    // Layer indices (same order as TagManager.asset)
    private const int Layer_Default         = 0;
    private const int Layer_TransparentFX   = 1;
    private const int Layer_IgnoreRaycast   = 2;
    private const int Layer_Water           = 4;
    private const int Layer_UI              = 5;
    private const int Layer_Player          = 8;
    private const int Layer_Enemy           = 9;
    private const int Layer_PlayerProj      = 10;
    private const int Layer_EnemyProj       = 11;
    private const int Layer_Environment     = 12;
    private const int Layer_Trap            = 13;
    private const int Layer_Interactable    = 15;  // 14 = empty slot, 15 = Interactable/ParryHitbox

    static PhysicsLayerCollisionSetup()
    {
        // ── Default (0) ──────────────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Default,          false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_IgnoreRaycast,    true );
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Player,            false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Enemy,             false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_PlayerProj,        false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_EnemyProj,         false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Environment,       false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Trap,              false);
        Physics2D.IgnoreLayerCollision(Layer_Default,          Layer_Interactable,      false);

        // ── IgnoreRaycast (2) ───────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_IgnoreRaycast,   Layer_Default,          true );
        Physics2D.IgnoreLayerCollision(Layer_IgnoreRaycast,   Layer_IgnoreRaycast,    true );

        // ── Player (8) ──────────────────────────────────────────────
        // "—" no self-collision check
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_Player,           true );  // no self-hit
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_Enemy,            false); // ✅ Player ↔ Enemy
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_PlayerProj,       true );  // — Player ↔ PlayerProj (no hit)
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_EnemyProj,        false); // ✅ Player ↔ EnemyProj
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_Environment,      false); // ✅ Player ↔ Environment
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_Trap,             false); // ✅ Player ↔ Trap
        Physics2D.IgnoreLayerCollision(Layer_Player,           Layer_Interactable,     true ); // — (not in matrix)

        // ── Enemy (9) ───────────────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_Enemy,            true );  // no self-hit
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_Player,           false); // ✅ Enemy ↔ Player
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_PlayerProj,       false); // ✅ Enemy ↔ PlayerProj
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_EnemyProj,        true );  // — Enemy ↔ EnemyProj
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_Environment,      false); // ✅ Enemy ↔ Environment
        Physics2D.IgnoreLayerCollision(Layer_Enemy,             Layer_Trap,             true );  // — Enemy ↔ Trap

        // ── PlayerProjectile (10) ───────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_PlayerProj,        Layer_PlayerProj,       true );  // no self-hit
        Physics2D.IgnoreLayerCollision(Layer_PlayerProj,        Layer_Enemy,            false); // ✅ PlayerProj ↔ Enemy
        Physics2D.IgnoreLayerCollision(Layer_PlayerProj,        Layer_Player,           true );  // — (duplicate of Player/Proj — true)
        Physics2D.IgnoreLayerCollision(Layer_PlayerProj,        Layer_EnemyProj,        true );  // — PlayerProj ↔ EnemyProj
        Physics2D.IgnoreLayerCollision(Layer_PlayerProj,        Layer_Environment,      false); // ✅ PlayerProj ↔ Environment

        // ── EnemyProjectile (11) ────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_EnemyProj,         Layer_EnemyProj,        true );  // no self-hit
        Physics2D.IgnoreLayerCollision(Layer_EnemyProj,         Layer_Player,           false); // ✅ EnemyProj ↔ Player
        Physics2D.IgnoreLayerCollision(Layer_EnemyProj,         Layer_Enemy,            true );  // — (duplicate of Enemy/EnemyProj — true)
        Physics2D.IgnoreLayerCollision(Layer_EnemyProj,         Layer_PlayerProj,       true );  // — (duplicate of Proj/Proj — true)
        Physics2D.IgnoreLayerCollision(Layer_EnemyProj,         Layer_Environment,      false); // ✅ EnemyProj ↔ Environment

        // ── Environment (12) ─────────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_Environment,   Layer_Player,           false);
        Physics2D.IgnoreLayerCollision(Layer_Environment,   Layer_Enemy,            false);
        Physics2D.IgnoreLayerCollision(Layer_Environment,   Layer_PlayerProj,       false);
        Physics2D.IgnoreLayerCollision(Layer_Environment,   Layer_EnemyProj,        false);

        // ── Trap (13) ───────────────────────────────────────────────
        Physics2D.IgnoreLayerCollision(Layer_Trap,           Layer_Player,           false); // ✅ Player ↔ Trap
        Physics2D.IgnoreLayerCollision(Layer_Trap,           Layer_Enemy,            true );  // — Enemy ↔ Trap

        // ── ParryHitbox / Interactable (15) ─────────────────────────
        // Per DisDoc Appendix B: ParryHitbox only collides with Enemy layer.
        // Projectile reflection is handled by tag "ParryHitbox" in ProjectileView
        // (works across layer boundaries).
        Physics2D.IgnoreLayerCollision(Layer_Interactable,  Layer_Player,       true );
        Physics2D.IgnoreLayerCollision(Layer_Interactable,  Layer_PlayerProj,   true );
        Physics2D.IgnoreLayerCollision(Layer_Interactable,  Layer_EnemyProj,    true );
        Physics2D.IgnoreLayerCollision(Layer_Interactable,  Layer_Environment,  true );
        Physics2D.IgnoreLayerCollision(Layer_Interactable,  Layer_Trap,         true );

        Debug.Log("[PhysicsLayerCollisionSetup] 2D layer collision matrix configured from DisDoc Appendix B.");
    }
}
