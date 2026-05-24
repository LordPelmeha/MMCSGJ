#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using HalfEmpty.Presentation.Player;
namespace HalfEmpty.Presentation.Vision
{
/// <summary>
/// Controls the Fog of War overlay and vision mode switching.
/// Uses URP 2D Lights as the primary implementation path:
/// a PointLight2D on the player with two radii (inner / outer) per FormConfig.
/// </summary>
public class VisionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D? _playerLight;
    [SerializeField] private SpriteRenderer? _fogOverlay;
    [Header("Config")]
    [SerializeField] private VisionConfigSO? _visionConfig;
    [SerializeField] private FormConfigSO? _headFormConfig;
    [SerializeField] private FormConfigSO? _bodyFormConfig;
    private bool _fullVisionMode;
    private FormType _currentForm = FormType.Body;
    private float _currentLightRadius;
    private void Start()
    {
        // Auto-find Light2D on same GameObject
        if (_playerLight == null)
            _playerLight = GetComponent<Light2D>();
    }
    private void Update()
    {
        if (_playerLight == null) return;
        if (_fullVisionMode)
        {
            _playerLight.intensity = 0f;
            if (_fogOverlay != null) _fogOverlay.enabled = false;
        }
    }
    /// <summary>Switch vision mode (called on form switch).</summary>
    public void SetForm(FormType form)
    {
        _currentForm = form;
        _fullVisionMode = form == FormType.Head;
        if (_playerLight == null) return;
        if (_fullVisionMode)
        {
            _playerLight.intensity = 0f;
            if (_fogOverlay != null) _fogOverlay.enabled = false;
        }
        else
        {
            _playerLight.intensity = 1f;
            if (_fogOverlay != null) _fogOverlay.enabled = true;
            float targetRadius = _bodyFormConfig != null
                ? _bodyFormConfig.outerVisionRadius
                : (_visionConfig != null ? _visionConfig.outerRadius : 5f);
            _currentLightRadius = Mathf.Lerp(_currentLightRadius, targetRadius, Time.deltaTime * 5f);
            _playerLight.pointLightOuterRadius = _currentLightRadius;
        }
    }
}
}
