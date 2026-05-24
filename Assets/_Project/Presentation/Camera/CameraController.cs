#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using UnityEngine;
using HalfEmpty.Presentation.Player;
using HalfEmpty.Infrastructure.Input;
using HalfEmpty.Presentation.Enemies;
namespace HalfEmpty.Presentation.Camera
{
/// <summary>
/// Follows the player and adds cursor-parallax during the Head form.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform? _playerTarget;
    [SerializeField] private CameraConfigSO? _config;
    [Header("Settings")]
    [SerializeField] private float _defaultOrthoSize = 6f;
    private UnityEngine.Camera? _cameraComponent;
    private IInputProvider? _inputProvider;
    private FormType _currentForm = FormType.Body;
    private Vector3 _camVelocity;
    private void Awake()
    {
        _cameraComponent = GetComponent<UnityEngine.Camera>();
        if (_cameraComponent == null)
            _cameraComponent = UnityEngine.Camera.main;
        _inputProvider = Object.FindObjectOfType<UnityInputProvider>();
    }
    private void LateUpdate()
    {
        if (_playerTarget == null || _cameraComponent == null) return;
        // Determine current form from PlayerController
        var playerCtrl = _playerTarget.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            _currentForm = playerCtrl.CurrentForm;
        }
        // Switch ortho size
        float targetSize = _config != null
            ? (_currentForm == FormType.Head ? _config.headFormOrthoSize : _config.bodyFormOrthoSize)
            : _defaultOrthoSize;
        _cameraComponent.orthographicSize = Mathf.Lerp(
            _cameraComponent.orthographicSize, targetSize, Time.deltaTime * 5f);
        // Follow player
        Vector3 targetPos = _playerTarget.position;
        targetPos.z = transform.position.z;
        // Cursor parallax for Head form
        if (_currentForm == FormType.Head && _config != null && _inputProvider != null)
        {
            Vector2 mousePos = _inputProvider.MouseWorldPosition;
            Vector2 playerPos = _playerTarget.position;
            Vector2 offset = (mousePos - playerPos) * _config.headFormCursorInfluence * 0.1f;
            targetPos += new Vector3(offset.x, offset.y, 0f);
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _camVelocity, 1f / (_config != null ? _config.headFormFollowSmoothing : 5f));
    }
}
}
