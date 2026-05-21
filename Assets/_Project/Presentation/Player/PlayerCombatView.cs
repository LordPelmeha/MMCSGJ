#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Presentation.Combat;
using UnityEngine;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// Handles shooting and parrying for the player.
/// </summary>
public class PlayerCombatView : MonoBehaviour
{
    private ProjectileFactory? _projectileFactory;
    private ProjectilePool? _currentPool;
    [Header("Combat References")]
    [SerializeField] private Transform? _firePoint;
    [SerializeField] private ParryHitboxView? _parryHitbox;
    [Header("Config")]
    [SerializeField] private FormConfigSO? _headFormConfig;
    [SerializeField] private FormConfigSO? _bodyFormConfig;
    [SerializeField] private PlayerConfigSO? _playerConfig;
    [SerializeField] private FormType _currentForm;
    private float _shootCooldownTimer;
    private float _parryCooldownTimer;
    private bool _isParrying;
    /// <summary>Raised after a shot is fired.</summary>
    public event Action? OnShot;
    /// <summary>Raised after a successful parry.</summary>
    public event Action? OnParry;
    /// <summary>Set the active form for damage/speed calculations.</summary>
    public void SetForm(FormType form)
    {
        _currentForm = form;
    }
}
}