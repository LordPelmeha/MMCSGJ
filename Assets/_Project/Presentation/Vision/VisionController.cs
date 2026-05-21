#nullable enable
using HalfEmpty.Infrastructure.Configs;
using UnityEngine;

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
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D? _playerLight;
    [SerializeField] private SpriteRenderer? _fogOverlay;
    [Header("Config")]
    [SerializeField] private VisionConfigSO? _visionConfig;
    [SerializeField] private FormConfigSO? _headFormConfig;
    [SerializeField] private FormConfigSO? _bodyFormConfig;
    private bool _fullVisionMode;
    private void Update()
    {
        if (_playerLight == null) return;
        if (_fullVisionMode)
        {
            // Head form — show everything.
            _playerLight.intensity = 0f;
            if (_fogOverlay != null) _fogOverlay.enabled = false;
            }
        }
    }
}
