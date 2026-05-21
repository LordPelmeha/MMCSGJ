#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;
namespace HalfEmpty.Infrastructure.Input
{
/// <summary>
/// Unity Input System-backed implementation of IInputProvider.
/// </summary>
public class UnityInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Input System (auto-assigned)")]
    [SerializeField] private InputActionReference? _moveAction;
    [SerializeField] private InputActionReference? _lookAction;
    [SerializeField] private InputActionReference? _shootAction;
    [SerializeField] private InputActionReference? _parryAction;
    [SerializeField] private InputActionReference? _markAction;
    [SerializeField] private InputActionReference? _switchFormAction;
    [SerializeField] private InputActionReference? _pauseAction;
    [SerializeField] private InputActionReference? _jumpAction;
    [SerializeField] private InputActionReference? _dashAction;
    [Header("Fallback Keys")]
    [SerializeField] private KeyCode _fallbackParryKey = KeyCode.F;
    [SerializeField] private KeyCode _fallbackMarkKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _fallbackSwitchKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _fallbackPauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode _fallbackJumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _fallbackDashKey = KeyCode.LeftControl;
    private Camera? _mainCamera;
    /// <summary>Horizontal movement axis (-1 ... 1).</summary>
    public float HorizontalAxis
    {
        get
        {
            if (_moveAction != null && _moveAction.action != null && _moveAction.action.enabled)
            {
                var v = _moveAction.action.ReadValue<Vector2>();
                return v.x;
            }
            return 0f;
        }
    }
    public bool JumpPressed => false;
    public bool DashPressed => false;
    public bool ShootPressed => false;
    public bool ParryPressed => false;
    public bool MarkPressed => false;
    public bool SwitchFormPressed => false;
    public bool SwitchFormReleased => false;
    public bool PausePressed => false;
    public Vector2 MouseWorldPosition => Vector2.zero;
}
}
