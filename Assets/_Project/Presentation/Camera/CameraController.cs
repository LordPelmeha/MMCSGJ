#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using UnityEngine;
namespace HalfEmpty.Presentation.Camera {
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
    private FormType _currentForm = FormType.Body;
    private void Awake()
    {
        _cameraComponent = GetComponent<UnityEngine.Camera>();
        if (_cameraComponent == null)
            _cameraComponent = UnityEngine.Camera.main;
    }
}
}