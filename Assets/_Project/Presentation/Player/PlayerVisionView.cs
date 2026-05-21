#nullable enable
using HalfEmpty.Domain.Enums;
using HalfEmpty.Infrastructure.Configs;
using UnityEngine;
using HalfEmpty.Presentation.Vision;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// Controls vision mode (full or limited with Fog of War radii).
/// </summary>
public class PlayerVisionView
{
    private readonly VisionController? _visionController;
    public PlayerVisionView(VisionController? visionController)
    {
        _visionController = visionController;
    }
}
}