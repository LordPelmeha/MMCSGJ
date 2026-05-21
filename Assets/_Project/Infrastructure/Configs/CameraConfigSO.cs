#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for the camera.
/// </summary>
[CreateAssetMenu(menuName = "Configs/Camera Config", fileName = "NewCameraConfig")]
public class CameraConfigSO : ScriptableObject
{
    [Header("Head Form")]
    [Tooltip("Follow smoothing for the Head form camera.")]
    public float headFormFollowSmoothing = 5f;
    [Tooltip("How strongly the camera leans toward the mouse cursor.")]
    public float headFormCursorInfluence = 2f;
    [Tooltip("Camera orthographic size when the Head form is active.")]
    public float headFormOrthoSize = 8f;
    [Header("Body Form")]
    [Tooltip("Camera orthographic size when the Body form is active.")]
    public float bodyFormOrthoSize = 5f;
}
}