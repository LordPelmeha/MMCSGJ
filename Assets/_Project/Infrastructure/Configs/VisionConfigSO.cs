#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for the Fog of War / vision system.
/// </summary>
[CreateAssetMenu(menuName = "Configs/Vision Config", fileName = "NewVisionConfig")]
public class VisionConfigSO : ScriptableObject
{
    [Header("Radii")]
    [Tooltip("Inner clear zone radius (world units). Everything here is fully visible.")]
    public float innerRadius = 3f;
    [Tooltip("Outer dimmed zone radius (world units). Everything farther is near-darkness.")]
    public float outerRadius = 5f;
    [Header("Alpha")]
    [Tooltip("Alpha multiplier in the outer dimmed zone (0 = invisible).")]
    public float outerAlpha = 0.4f;
    [Tooltip("Alpha multiplier in full darkness (0 = fully transparent rendering).")]
    public float darknessAlpha = 1f;
    [Header("Marks")]
    [Tooltip("If true, Marked objects are visible even in the full darkness zone.")]
    public bool markThroughDarkness = true;
    [Header("Transition")]
    [Tooltip("Smoothness of transition between vision zones (lower = sharper edge).")]
    public float transitionSmoothness = 0.5f;
}
}