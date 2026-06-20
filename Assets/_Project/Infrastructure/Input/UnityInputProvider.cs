#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;
namespace HalfEmpty.Infrastructure.Input
{
/// <summary>
/// Input System-backed implementation of IInputProvider.
/// </summary>
public class UnityInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference? _moveAction;
    [SerializeField] private InputActionReference? _lookAction;
    [SerializeField] private InputActionReference? _shootAction;
    [SerializeField] private InputActionReference? _parryAction;
    [SerializeField] private InputActionReference? _markAction;
    [SerializeField] private InputActionReference? _switchFormAction;
    [SerializeField] private InputActionReference? _pauseAction;
    [SerializeField] private InputActionReference? _jumpAction;
    [SerializeField] private InputActionReference? _dashAction;
    private Camera? _mainCamera;
    private static Keyboard? _keyboard => Keyboard.current;
    private InputAction? MoveAction => _moveAction?.action;
    private InputAction? ShootAction => _shootAction?.action;
    private InputAction? ParryAction => _parryAction?.action;
    private InputAction? MarkAction => _markAction?.action;
    private InputAction? SwitchFormAction => _switchFormAction?.action;
    private InputAction? PauseAction => _pauseAction?.action;
    private InputAction? JumpAction => _jumpAction?.action;
    private InputAction? DashAction => _dashAction?.action;
    #nullable disable
    public float HorizontalAxis
    {
        get
        {
            // Try Input Action first
            if (MoveAction != null && MoveAction.enabled)
            {
                var v = MoveAction.ReadValue<Vector2>();
                if (Mathf.Abs(v.x) > 0.01f) { Debug.Log($"[Input] HorizontalAxis={v.x:F2} via action"); return v.x; }
            }
            // Fallback to keyboard state directly
            if (_keyboard != null)
            {
                var a = _keyboard.aKey.isPressed;
                var d = _keyboard.dKey.isPressed;
                if (a && !d) { Debug.Log("[Input] HorizontalAxis=-1 via keyboard A"); return -1f; }
                if (d && !a) { Debug.Log("[Input] HorizontalAxis=+1 via keyboard D"); return 1f; }
            }
            // Debug: log when keyboard is null
            if (_keyboard == null) Debug.LogWarning("[Input] Keyboard.current is null! Cannot read fallback input.");
            return 0f;
        }
    }
    public bool JumpPressed
    {
        get
        {
            bool action = JumpAction?.triggered ?? false;
            bool keyboard = _keyboard?.spaceKey.wasPressedThisFrame ?? false;
            if (action || keyboard) Debug.Log($"[Input] JumpPressed! action={action} keyboard={keyboard}");
            return action || keyboard;
        }
    }
    public bool DashPressed
    {
        get
        {
            bool action = DashAction?.triggered ?? false;
            bool keyboard = _keyboard?.leftCtrlKey.wasPressedThisFrame ?? false;
            if (action || keyboard) Debug.Log($"[Input] DashPressed! action={action} keyboard={keyboard}");
            return action || keyboard;
        }
    }
    public bool ShootPressed
    {
        get
        {
            bool action = ShootAction?.triggered ?? false;
            bool mouse = Mouse.current?.leftButton.wasPressedThisFrame ?? false;
            if (action || mouse) Debug.Log($"[Input] ShootPressed! action={action} mouse={mouse}");
            return action || mouse;
        }
    }
    public bool ParryPressed
    {
        get
        {
            bool action = ParryAction?.triggered ?? false;
            bool keyboard = _keyboard?.fKey.wasPressedThisFrame ?? false;
            if (action || keyboard) Debug.Log($"[Input] ParryPressed! action={action} keyboard={keyboard}");
            return action || keyboard;
        }
    }
    public bool MarkPressed
    {
        get
        {
            bool action = MarkAction?.triggered ?? false;
            bool mouse = Mouse.current?.rightButton.wasPressedThisFrame ?? false;
            if (action || mouse) Debug.Log($"[Input] MarkPressed! action={action} mouse={mouse}");
            return action || mouse;
        }
    }
    public bool SwitchFormPressed
    {
        get
        {
            bool action = SwitchFormAction?.triggered ?? false;
            bool keyboard = _keyboard?.leftShiftKey.wasPressedThisFrame ?? false;
            if (action || keyboard) Debug.Log($"[Input] SwitchFormPressed! action={action} keyboard={keyboard}");
            return action || keyboard;
        }
    }
    public bool SwitchFormReleased
    {
        get
        {
            bool keyboard = _keyboard?.leftShiftKey.wasReleasedThisFrame ?? false;
            if (keyboard) Debug.Log("[Input] SwitchFormReleased!");
            return keyboard;
        }
    }
    public bool PausePressed
    {
        get
        {
            bool action = PauseAction?.triggered ?? false;
            bool keyboard = _keyboard?.escapeKey.wasPressedThisFrame ?? false;
            if (action || keyboard) Debug.Log($"[Input] PausePressed! action={action} keyboard={keyboard}");
            return action || keyboard;
        }
    }
    #nullable restore
    public Vector2 MouseWorldPosition
    {
        get
        {
            if (_mainCamera == null) _mainCamera = UnityEngine.Camera.main;
            if (_mainCamera == null) return Vector2.zero;
            var mouse = Mouse.current;
            if (mouse == null) return Vector2.zero;
            var screenPos = mouse.position.ReadValue();
            return _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _mainCamera.nearClipPlane));
        }
    }
    private void OnEnable()
    {
        Debug.Log("[Input] UnityInputProvider.OnEnable() - enabling actions");
        _moveAction?.action?.Enable();
        _shootAction?.action?.Enable();
        _parryAction?.action?.Enable();
        _markAction?.action?.Enable();
        _switchFormAction?.action?.Enable();
        _pauseAction?.action?.Enable();
        _jumpAction?.action?.Enable();
        _dashAction?.action?.Enable();
    }
    private void OnDisable()
    {
        _moveAction?.action?.Disable();
        _shootAction?.action?.Disable();
        _parryAction?.action?.Disable();
        _markAction?.action?.Disable();
        _switchFormAction?.action?.Disable();
        _pauseAction?.action?.Disable();
        _jumpAction?.action?.Disable();
        _dashAction?.action?.Disable();
    }
}
}