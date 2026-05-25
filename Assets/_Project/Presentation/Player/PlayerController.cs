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

namespace HalfEmpty.Presentation.Player
{
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

        [Header("Form Transition Animation")]
        [SerializeField] private float _toHeadTransitionDuration = 0.25f;
        [SerializeField] private float _toBodyTransitionDuration = 0.25f;

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

        // Form transition state
        private bool _isFormTransitioning;

        // Visuals
        private Animator? _animator;
        private SpriteRenderer? _spriteRenderer;
        private bool _isFacingRight = true;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        private static readonly int ToHeadTriggerHash = Animator.StringToHash("ToHeadTrigger");
        private static readonly int ToBodyTriggerHash = Animator.StringToHash("ToBodyTrigger");

        /// <summary>Currently active form.</summary>
        public FormType CurrentForm => _currentForm;

        /// <summary>True while form transition animation is playing.</summary>
        public bool IsFormTransitioning => _isFormTransitioning;

        /// <summary>Switch the active form and notify systems.</summary>
        public void SetForm(FormType form)
        {
            if (_currentForm == form) return;
            _currentForm = form;
        }

        public FormConfigSO? BodyConfig => _config?.bodyFormConfig;
        public FormConfigSO? HeadConfig => _config?.headFormConfig;
        public FormConfigSO? ActiveFormConfig =>
            _currentForm == FormType.Head ? _config?.headFormConfig : _config?.bodyFormConfig;
        public PlayerMovementView? MovementView => _movementView;
        public IInputProvider? InputProvider => _inputProvider;
        public PlayerConfigSO? Config => _config;

        private void Awake()
        {
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            if (_mainCollider == null) _mainCollider = GetComponent<Collider2D>();

            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_movementView == null)
                _movementView = new PlayerMovementView();

            if (_rb != null)
                _movementView.Setup(_rb, _groundCheck, _groundCheckRadius, _environmentLayer);

            if (_visionView == null)
            {
                var visionController = FindObjectOfType<VisionController>();
                _visionView = new PlayerVisionView(visionController);
            }

            if (_animationView == null)
            {
                var headRenderer = _headPart?.GetComponentInChildren<SpriteRenderer>(true);
                var bodyRenderer = _bodyPart?.GetComponentInChildren<SpriteRenderer>(true);
                _animationView = new PlayerAnimationView(_animator, headRenderer, bodyRenderer);
            }

            if (_inputProvider == null)
            {
                _inputProvider = GetComponent<UnityInputProvider>();
                if (_inputProvider == null)
                {
                    _inputProvider = FindObjectOfType<UnityInputProvider>();
                    if (_inputProvider == null)
                    {
                        var go = new GameObject("UnityInputProvider");
                        _inputProvider = go.AddComponent<UnityInputProvider>();
                    }
                }
            }

            _formStateMachine = new StateMachine();
            _formStateMachine.ChangeState(new BodyFormState(this));

            if (_headPart != null) _headPart.gameObject.SetActive(false);
            if (_bodyPart != null) _bodyPart.gameObject.SetActive(true);

            _animator?.SetFloat(SpeedHash, 0f);
            _animator?.ResetTrigger(JumpTriggerHash);
            _animator?.ResetTrigger(ToHeadTriggerHash);
            _animator?.ResetTrigger(ToBodyTriggerHash);

            SetFacing(true);
        }

        private void Start()
        {
            if (_projectilePool != null)
            {
                _projectileFactory = new ProjectileFactory(_projectilePool);
            }

            if (_combatView != null && _projectileFactory != null && _projectilePool != null)
            {
                _combatView.SetProjectileFactory(_projectileFactory);
                _combatView.SetProjectilePool(_projectilePool);
            }
        }

        private void Update()
        {
            if (!_isFormTransitioning)
            {
                _formStateMachine?.Update();
            }

            HandleButtons();
            UpdateDash(Time.deltaTime);
            UpdateFacing();
            UpdateRunAnimation();
        }

        private void FixedUpdate()
        {
            if (!_isFormTransitioning)
            {
                _formStateMachine?.FixedUpdate();
            }

            if (_rb != null && _dashing)
            {
                var t = Mathf.Clamp01((Time.fixedTime - _dashStartTime) / _dashDuration);
                _rb.transform.position = Vector2.Lerp(_dashStartPos, _dashEndPos, t);
                if (t >= 1f) StopDash();
            }
        }

        private void HandleButtons()
        {
            if (_inputProvider == null) return;

            if (_inputProvider.PausePressed)
            {
                if (Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
                    OnPausePressed();
            }

            if (_isFormTransitioning)
                return;

            if (_inputProvider.JumpPressed && _rb != null)
            {
                var activeConfig = ActiveFormConfig;
                if (activeConfig != null && activeConfig.canJump)
                {
                    var strategy = _movementView?.GetStrategy();
                    if (strategy != null && strategy.CanJump)
                    {
                        strategy.Jump(_rb, activeConfig.jumpForce);
                        _animator?.SetTrigger(JumpTriggerHash);
                    }
                }
            }

            if (_inputProvider.DashPressed && !_dashing && _rb != null)
            {
                var activeConfig = ActiveFormConfig;
                if (activeConfig != null && activeConfig.canDash)
                {
                    var strategy = _movementView?.GetStrategy();
                    if (strategy != null && strategy.CanDash)
                    {
                        float dir = _inputProvider.HorizontalAxis;
                        if (Mathf.Abs(dir) < 0.01f)
                            dir = _isFacingRight ? 1f : -1f;

                        StartDash(dir, activeConfig.dashDistance, activeConfig.dashDuration);
                    }
                }
            }

            if (_inputProvider.ShootPressed && _combatView != null)
            {
                var bodyConfig = BodyConfig;
                if (bodyConfig != null)
                    _combatView.HandleShoot(_inputProvider, bodyConfig);
            }

            if (_inputProvider.ParryPressed && _combatView != null)
            {
                _combatView.HandleParry();
            }

            if (_inputProvider.SwitchFormPressed)
            {
                SwitchForm();
            }
        }

        private void StartDash(float direction, float distance, float duration)
        {
            if (_rb == null) return;

            _dashStartPos = _rb.transform.position;
            _dashEndPos = _dashStartPos + new Vector2(direction * distance, 0f);
            _dashStartTime = Time.time;
            _dashDuration = duration;
            _dashing = true;

            Debug.Log($"[PlayerController] Dash started! dir={direction:F1} dist={distance:F1} dur={duration:F2} start={_dashStartPos} end={_dashEndPos}");

            var v = _rb.linearVelocity;
            v.x = direction * (distance / duration);
            v.y = 0f;
            _rb.linearVelocity = v;
        }

        private void UpdateDash(float deltaTime)
        {
            if (!_dashing) return;
        }

        private void StopDash()
        {
            Debug.Log("[PlayerController] Dash ended.");
            _dashing = false;

            if (_rb != null)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y);
            }
        }

        private void StopMovementForTransition()
        {
            _dashing = false;

            if (_rb != null)
            {
                var velocity = _rb.linearVelocity;
                velocity.x = 0f;
                _rb.linearVelocity = velocity;
            }

            _animator?.SetFloat(SpeedHash, 0f);
            _animator?.ResetTrigger(JumpTriggerHash);
        }

        private void UpdateRunAnimation()
        {
            if (_animator == null)
                return;

            float speed = 0f;

            if (!_isFormTransitioning && _rb != null && _currentForm == FormType.Body)
            {
                speed = Mathf.Abs(_rb.linearVelocity.x);
            }

            _animator.SetFloat(SpeedHash, speed);
        }

        private void UpdateFacing()
        {
            if (_inputProvider == null) return;
            if (_currentForm != FormType.Body) return;
            if (_isFormTransitioning) return;

            float horizontal = _inputProvider.HorizontalAxis;

            if (horizontal > 0.01f)
                SetFacing(true);
            else if (horizontal < -0.01f)
                SetFacing(false);
        }

        private void SetFacing(bool faceRight)
        {
            _isFacingRight = faceRight;

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = !faceRight;
        }

        private void SwitchForm()
        {
            if (_config == null) return;
            if (_formStateMachine == null) return;
            if (_isFormTransitioning) return;

            StartCoroutine(SwitchFormRoutine());
        }

        private IEnumerator SwitchFormRoutine()
        {
            _isFormTransitioning = true;
            _formWasSwitchedThisFrame = true;

            var newForm = _currentForm == FormType.Body ? FormType.Head : FormType.Body;

            _markView?.ClearAllMarks();

            StopMovementForTransition();

            _animator?.ResetTrigger(ToHeadTriggerHash);
            _animator?.ResetTrigger(ToBodyTriggerHash);

            if (newForm == FormType.Head)
            {
                _animator?.SetTrigger(ToHeadTriggerHash);
                yield return new WaitForSeconds(_toHeadTransitionDuration);
            }
            else
            {
                _animator?.SetTrigger(ToBodyTriggerHash);
                yield return new WaitForSeconds(_toBodyTransitionDuration);
            }

            ApplyForm(newForm);

            _isFormTransitioning = false;
            _formWasSwitchedThisFrame = false;
        }

        private void ApplyForm(FormType newForm)
        {
            if (_config == null) return;
            if (_formStateMachine == null) return;

            SetForm(newForm);

            if (_markView != null)
            {
                var newConfig = newForm == FormType.Body ? _config.bodyFormConfig : _config.headFormConfig;
                _markView.SetActiveFormConfig(newConfig);
            }

            _combatView?.SetForm(newForm);

            var newState = newForm == FormType.Body
                ? (IState)new BodyFormState(this)
                : (IState)new HeadFormState(this);

            _formStateMachine.ChangeState(newState);

            if (_headPart != null) _headPart.gameObject.SetActive(newForm == FormType.Head);
            if (_bodyPart != null) _bodyPart.gameObject.SetActive(newForm == FormType.Body);

            if (newForm == FormType.Head)
            {
                _animator?.SetFloat(SpeedHash, 0f);
            }

            Debug.Log($"[PlayerController] Form switched to: {newForm}");
        }

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