#nullable enable

using System;
using UnityEditor;
using UnityEngine;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;

/// <summary>
/// One-shot editor utility that creates all ScriptableObject config assets
/// and supporting prefabs for the "Half Empty" project.
/// Run via menu: Tools/Half Empty/🛠 Create All Config Assets & Player Prefab
///
/// Only assets that do not already exist will be created (safe to re-run).
/// </summary>
public static class HalfEmptyAssetCreator
{
    private const string ConfigRoot  = "Assets/Configs/";
    private const string PlayerRoot  = ConfigRoot + "Player/";
    private const string EnemyRoot   = ConfigRoot + "Enemies/";
    private const string ProjRoot    = ConfigRoot + "Projectiles/";
    private const string CameraRoot  = ConfigRoot + "Camera/";
    private const string VisionRoot  = ConfigRoot + "Vision/";
    private const string EventsRoot  = ConfigRoot + "Events/";
    private const string PrefabRoot  = "Assets/Prefabs/";

    [MenuItem("Tools/Half Empty/🛠 Create All Config Assets & Player Prefab")]
    private static void CreateAllAssets()
    {
        Undo.RecordObject(null, "Create Half Empty Assets");

        CreateFolder(ConfigRoot);
        CreateFolder(PlayerRoot);
        CreateFolder(EnemyRoot);
        CreateFolder(ProjRoot);
        CreateFolder(CameraRoot);
        CreateFolder(VisionRoot);
        CreateFolder(EventsRoot);
        CreateFolder(PrefabRoot + "Player");
        CreateFolder(PrefabRoot + "Enemies");
        CreateFolder(PrefabRoot + "Projectiles");

        // ── Player Configs ────────────────────────────────────────
        CreateAsset<FormConfigSO>(PlayerRoot + "HeadFormConfig.asset",   c =>
        {
            c.maxHP             = 100;
            c.moveSpeed         = 2f;
            c.canJump           = false;
            c.jumpForce         = 12f;
            c.canDash           = false;
            c.dashDistance      = 4f;
            c.dashDuration      = 0.15f;
            c.dashCooldown      = 1f;
            c.dashInvincible    = true;
            c.shootDamage       = 25f;
            c.shootRate         = 0.4f;
            c.projectileSpeed   = 15f;
            c.damageMultiplier  = 1f;
            c.markDuration      = 10f;
            c.maxMarks          = 5;
            c.fullVision        = true;
            c.innerVisionRadius = 3f;
            c.outerVisionRadius = 5f;
            c.outerVisionAlpha  = 0.4f;
            Debug.Log("  ✓ HeadFormConfig");
        });

        CreateAsset<FormConfigSO>(PlayerRoot + "BodyFormConfig.asset",  c =>
        {
            c.maxHP             = 75;
            c.moveSpeed         = 6f;
            c.canJump           = true;
            c.jumpForce         = 12f;
            c.canDash           = true;
            c.dashDistance      = 4f;
            c.dashDuration      = 0.15f;
            c.dashCooldown      = 1f;
            c.dashInvincible    = true;
            c.shootDamage       = 12f;
            c.shootRate         = 0.3f;
            c.projectileSpeed   = 12f;
            c.damageMultiplier  = 0.5f;
            c.markDuration      = 10f;
            c.maxMarks          = 5;
            c.fullVision        = false;
            c.innerVisionRadius = 3f;
            c.outerVisionRadius = 5f;
            c.outerVisionAlpha  = 0.4f;
            Debug.Log("  ✓ BodyFormConfig");
        });

        CreateAsset<PlayerConfigSO>(PlayerRoot + "PlayerConfig.asset", c =>
        {
            c.formSwitchCooldown = 0.5f;
            c.parryWindow        = 0.3f;
            c.parryCooldown      = 0.5f;
            Debug.Log("  ✓ PlayerConfig");
        });

        // ── Enemy Configs ─────────────────────────────────────────
        CreateAsset<EnemyConfigSO>(EnemyRoot + "MeleeEnemyConfig.asset", c =>
        {
            c.hp                 = 50;
            c.moveSpeed          = 4f;
            c.detectionRange     = 10f;
            c.detectionAngle     = 180f;
            c.attackDamage       = 20f;
            c.attackRange        = 1.5f;
            c.attackCooldown     = 1f;
            c.canBeParried       = true;
            Debug.Log("  ✓ MeleeEnemyConfig");
        });

        CreateAsset<EnemyConfigSO>(EnemyRoot + "RangedEnemyConfig.asset", c =>
        {
            c.hp                 = 30;
            c.moveSpeed          = 0f;
            c.detectionRange     = 12f;
            c.detectionAngle     = 180f;
            c.attackDamage       = 15f;
            c.attackRange        = 0f;
            c.attackCooldown     = 1.5f;
            c.projectileSpeed    = 10f;
            c.fireRate           = 1.5f;
            c.canBeParried       = true;
            Debug.Log("  ✓ RangedEnemyConfig");
        });

        // ── Projectile Configs ────────────────────────────────────
        CreateAsset<ProjectileConfigSO>(ProjRoot + "PlayerProjectileConfig.asset", c =>
        {
            c.damage               = 25f;
            c.speed                = 15f;
            c.lifetime             = 5f;
            c.colliderSize         = new Vector2(0.3f, 0.3f);
            c.canBeParried         = true;
            c.reflectedSpeedMultiplier = 1.5f;
            c.targetLayer          = LayerMask.GetMask("Enemy", "Environment");
            Debug.Log("  ✓ PlayerProjectileConfig");
        });

        CreateAsset<ProjectileConfigSO>(ProjRoot + "EnemyProjectileConfig.asset", c =>
        {
            c.damage               = 15f;
            c.speed                = 10f;
            c.lifetime             = 5f;
            c.colliderSize         = new Vector2(0.3f, 0.3f);
            c.canBeParried         = true;
            c.reflectedSpeedMultiplier = 1.5f;
            c.targetLayer          = LayerMask.GetMask("Player", "Environment");
            Debug.Log("  ✓ EnemyProjectileConfig");
        });

        // ── Camera Config ─────────────────────────────────────────
        CreateAsset<CameraConfigSO>(CameraRoot + "CameraConfig.asset", c =>
        {
            c.headFormFollowSmoothing  = 5f;
            c.headFormCursorInfluence  = 2f;
            c.headFormOrthoSize        = 8f;
            c.bodyFormOrthoSize        = 5f;
            Debug.Log("  ✓ CameraConfig");
        });

        // ── Vision Config ─────────────────────────────────────────
        CreateAsset<VisionConfigSO>(VisionRoot + "VisionConfig.asset", c =>
        {
            c.innerRadius        = 3f;
            c.outerRadius        = 5f;
            c.outerAlpha         = 0.4f;
            c.darknessAlpha      = 1f;
            c.markThroughDarkness = true;
            c.transitionSmoothness = 0.5f;
            Debug.Log("  ✓ VisionConfig");
        });

        // ── Event SOs ─────────────────────────────────────────────
        CreateAsset<VoidEventSO>(EventsRoot + "OnPlayerDeath.asset",   c => { Debug.Log("  ✓ OnPlayerDeath"); });
        CreateAsset<VoidEventSO>(EventsRoot + "OnFormSwitched.asset",  c => { Debug.Log("  ✓ OnFormSwitched");  });
        CreateAsset<VoidEventSO>(EventsRoot + "OnParrySuccess.asset",  c => { Debug.Log("  ✓ OnParrySuccess");  });

        Debug.Log("[HalfEmptyAssetCreator] ✅ All config assets created successfully.");
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static void CreateFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            var parent = System.IO.Path.GetDirectoryName(path)!.Replace("\\", "/");
            var name   = System.IO.Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, name);
            Debug.Log($"  📁 Folder created: {path}");
        }
    }

    private static void CreateAsset<T>(string path, Action<T>? init = null) where T : ScriptableObject
    {
        if (AssetDatabase.LoadAssetAtPath<T>(path) != null)
        {
            Debug.Log($"  ⏭  Skipped (exists): {path}");
            return;
        }
        var asset = ScriptableObject.CreateInstance<T>();
        init?.Invoke(asset);
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
    }
}
