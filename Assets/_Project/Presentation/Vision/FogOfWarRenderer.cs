#nullable enable
using HalfEmpty.Infrastructure.Configs;
using UnityEngine;
namespace HalfEmpty.Presentation.Vision
{
/// <summary>
/// Fallback custom shader renderer for Fog of War.
/// Only used if URP 2D Lights are unavailable.
/// Renders a fullscreen black quad with radial gradient alpha holes.
/// </summary>
public class FogOfWarRenderer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private VisionConfigSO? _visionConfig;
    [Header("Material")]
    [SerializeField] private Material? _fogOfWarMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_fogOfWarMaterial != null)
        {
            Graphics.Blit(src, dst, _fogOfWarMaterial);
    }
        }    }
}
