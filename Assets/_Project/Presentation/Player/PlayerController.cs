#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Domain.Health;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Infrastructure.Input;
using HalfEmpty.Infrastructure.Factories;
using HalfEmpty.Infrastructure.Pools;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Player;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.Game;
using HalfEmpty.Presentation.Combat;
using HalfEmpty.Presentation.Vision;
using HalfEmpty.Presentation.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    [Header("Combat")]
    [SerializeField] private ProjectilePool? _projectilePool;
    [Header("Settings")]
    [SerializeField] private LayerMask _environmentLayer;
    [SerializeField] private Transform? _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    
    private HalfEmpty.Application.FSM.StateMachine? _formStateMachine;
    private FormType _currentForm = FormType.Body;
    private bool _formWasSwitchedThisFrame;
    // Dash state
    private bool _dashing;
    private Vector2 _dashStartPos;
    private Vector2 _dashEndPos;
    private float _dashStartTime;
    private float _dashDuration;
    private ProjectileFactory? _projectileFactory;

    /// <summary>Currently active form.</summary>
    public FormType CurrentForm => _currentForm;
    /// <summary>Switch the active form and notify systems.</summary>
    public void SetForm(FormType form)
    {
        if (_currentForm == form) return;
        _currentForm = form;
    }
    /// <summary>Get body form config (convenience accessor).</summary>
    public FormConfigSO? BodyConfig => _config?.bodyFormConfig;
    /// <summary>Get head form config (convenience accessor).</summary>
    public FormConfigSO? HeadConfig => _config?.headFormConfig;
    /// <summary>Currently active form config based on current form type.</summary>
    public FormConfigSO? ActiveFormConfig => _currentForm == FormType.Head ? _config?.headFormConfig : _config?.bodyFormConfig;
    /// <summary>Movement view (set by configuration).</summary>
    public PlayerMovementView? MovementView => _movementView;
    /// <summary>Input provider reference.</summary>
    public IInputProvider? InputProvider => _inputProvider;
    /// <summary>PlayerConfig reference for reading form settings.</summary>
    public PlayerConfigSO? Config => _config;

    private void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_mainCollider == null) _mainCollider = GetComponent<Collider2D>();
        // Ensure views are instantiated if not assigned via inspector
        if (_movementView == null)
            _movementView = new PlayerMovementView();
        // Always call Setup on movement view after potential instantiation
        if (_rb != null)
            _movementView.Setup(_rb, _groundCheck, _groundCheckRadius, _environmentLayer);
        if (_visionView == null)
        {
            var visionController = FindObjectOfType<VisionController>();
            _visionView = new PlayerVisionView(visionController);
        }
        if (_animationView == null)
        {
            // Find animator and sprite renderers on HeadPart and BodyPart
            var animator = GetComponentInChildren<Animator>();
            var headRenderer = _headPart?.GetComponent<SpriteRenderer>();
            var bodyRenderer = _bodyPart?.GetComponent<SpriteRenderer>();
            _animationView = new PlayerAnimationView(animator, headRenderer, bodyRenderer);
        }
        // Set up input provider
        if (_inputProvider == null)
        {
            _inputProvider = GetComponent<UnityInputProvider>();
            if (_inputProvider == null)
            {
                _inputProvider = FindObjectOfType<UnityInputProvider>();
                if (_inputProvider == null)
                {
                    // Create a new GameObject and add the UnityInputProvider component
                    var go = new GameObject("UnityInputProvider");
                    _inputProvider = go.AddComponent<UnityInputProvider>();
                }
            }
        }
        _formStateMachine = new StateMachine();
        _formStateMachine.ChangeState(new BodyFormState(this));
    }

    private void Start()
    {
        // Create projectile factory if pool is assigned
        if (_projectilePool != null)
        {
            _projectileFactory = new ProjectileFactory(_projectilePool);
        }
        // Wire up combat view with factory and pool
        if (_combatView != null && _projectileFactory != null && _projectilePool != null)
        {
            _combatView.SetProjectileFactory(_projectileFactory);
            _combatView.SetProjectilePool(_projectilePool);
        }
    }

    private void Update()
    {
        _formStateMachine?.Update();
        HandleButtons();
        UpdateDash(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _formStateMachine?.FixedUpdate();
        if (_rb != null && _dashing)
        {
            // Smooth dash position update during physics step
            var t = Mathf.Clamp01((Time.fixedTime - _dashStartTime) / _dashDuration);
            _rb.transform.position = Vector2.Lerp(_dashStartPos, _dashEndPos, t);
            if (t >= 1f) StopDash();
        }
    }

    /// <summary>Poll all action buttons each frame and dispatch to handlers.</summary>
    private void HandleButtons()
    {
        if (_inputProvider == null) return;
        // Jump
        if (_inputProvider.JumpPressed && _rb != null)
        {
            var activeConfig = ActiveFormConfig;
            if (activeConfig != null && activeConfig.canJump)
            {
                var strategy = _movementView?.GetStrategy();
                if (strategy != null && strategy.CanJump)
                    strategy.Jump(_rb, activeConfig.jumpForce);
            }
        }
        // Dash
        if (_inputProvider.DashPressed && !_dashing && _rb != null)
        {
            var activeConfig = ActiveFormConfig;
            if (activeConfig != null && activeConfig.canDash)
            {
                var strategy = _movementView?.GetStrategy();
                if (strategy != null && strategy.CanDash)
                {
                    float dir = _inputProvider.HorizontalAxis;
                    if (Mathf.Abs(dir) < 0.01f) dir = _rb.transform.localScale.x > 0 ? 1f : -1f;
                    StartDash(dir, activeConfig.dashDistance, activeConfig.dashDuration);
                }
            }
        }
        // Shoot
        if (_inputProvider.ShootPressed && _combatView != null)
        {
            var bodyConfig = BodyConfig;
            if (bodyConfig != null)
                _combatView.HandleShoot(_inputProvider, bodyConfig);
        }
        // Parry
        if (_inputProvider.ParryPressed && _combatView != null)
        {
            _combatView.HandleParry();
        }
        // Switch Form
        if (_inputProvider.SwitchFormPressed)
        {
            SwitchForm();
        }
        // Pause
        if (_inputProvider.PausePressed)
        {
            // Use legacy escape check here because Pause must work even when Time.timeScale=0
            if (Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
                OnPausePressed();
        }
    }

    /// <summary>Start a dash movement. Must be called from the main thread.</summary>
    private void StartDash(float direction, float distance, float duration)
    {
        if (_rb == null) return;
        _dashStartPos = _rb.transform.position;
        _dashEndPos = _dashStartPos + new Vector2(direction * distance, 0f);
        _dashStartTime = Time.time;
        _dashDuration = duration;
        _dashing = true;
        Debug.Log($"[PlayerController] Dash started! dir={direction:F1} dist={distance:F1} dur={duration:F2} start={_dashStartPos} end={_dashEndPos}");
        // Apply initial dash velocity impulse
        var v = _rb.linearVelocity;
        v.x = direction * (distance / duration);
        v.y = 0f;
        _rb.linearVelocity = v;
    }

    /// <summary>Update dash position during physics step.</summary>
    private void UpdateDash(float deltaTime)
    {
        if (!_dashing) return;
        // Dash progress is handled in FixedUpdate via geometry lerp
    }

    /// <summary>Called internally once the dash destination has been reached.</summary>
    private void StopDash()
    {
        Debug.Log("[PlayerController] Dash ended.");
        _dashing = false;
        if (_rb != null)
        {
            // Normalize velocity after dash — restore standard horizontal movement speed
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y);
        }
    }

    /// <summary>Switch to the other form.</summary>
    private void SwitchForm()
    {
        if (_config == null) return;
        if (_formStateMachine == null) return;
        var newForm = _currentForm == FormType.Body ? FormType.Head : FormType.Body;
        // Expire marks when switching forms
        _markView?.ClearAllMarks();
        // Update MarkView's active form config
        if (_markView != null)
        {
            var newConfig = newForm == FormType.Body ? _config.bodyFormConfig : _config.headFormConfig;
            _markView.SetActiveFormConfig(newConfig);
        }
        // Update combat view's form
        _combatView?.SetForm(newForm);
        var newState = newForm == FormType.Body ? (IState)new BodyFormState(this) : (IState)new HeadFormState(this);
        _formStateMachine.ChangeState(newState);
        // Toggle form visuals
        if (_headPart != null) _headPart.gameObject.SetActive(newForm == FormType.Head);
        if (_bodyPart != null) _bodyPart.gameObject.SetActive(newForm == FormType.Body);
        Debug.Log($"[PlayerController] Form switched to: {newForm}");
    }

    /// <summary>Signal the game state machine to pause (called when Escape is pressed).</summary>
    private void OnPausePressed()
    {
        Debug.Log("[PlayerController] Pause requested.");
        var controller = Object.FindFirstObjectByType<GameFlowController>();
        if (controller != null)
        {
            controller.GameFlowSMRef?.ChangeState(new PausedState());
        }
    }
}
}