#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Infrastructure.Input;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Player;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Presentation.Combat;
using UnityEngine;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// Main coordinator for the player. Owns the form state machine and wires up all sub-systems.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerConfigSO? _config;
    [Header("Physics References")]
    [SerializeField] private Rigidbody2D? _rb;
    [SerializeField] private Collider2D? _mainCollider;
    [Header("Form Parts")]
    [Tooltip("Parent GameObjects for each form's visuals (SpriteRenderer + Animator only).")]
    [SerializeField] private Transform? _headPart;
    [SerializeField] private Transform? _bodyPart;
    [Header("Sub-System Views")]
    [SerializeField] private PlayerMovementView? _movementView;
    [SerializeField] private PlayerCombatView? _combatView;
    [SerializeField] private PlayerHealthView? _healthView;
    [SerializeField] private PlayerVisionView? _visionView;
    [SerializeField] private PlayerAnimationView? _animationView;
    [SerializeField] private MarkView? _markView;
    [SerializeField] private ParryHitboxView? _parryHitbox;
    [Header("Input")]
    [SerializeField] private IInputProvider? _inputProvider;
    [Header("Settings")]
    [SerializeField] private LayerMask _environmentLayer;
    [SerializeField] private Transform? _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    private HalfEmpty.Application.FSM.StateMachine? _formStateMachine;
    private FormType _currentForm = FormType.Body;
    private bool _formWasSwitchedThisFrame;
    /// <summary>Currently active form.</summary>
    public FormType CurrentForm => _currentForm;
    private void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
    }
}
}