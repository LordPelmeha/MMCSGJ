#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using UnityEngine;
using System.Collections.Generic;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// Handles the Marking system: places a mark under the cursor, tracks max marks,
/// and raises events for the HUD and markable components.
/// </summary>
public class MarkView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnityEngine.Camera? _camera;
    [SerializeField] private Transform? _cursorMarkerPrefab;
    [Header("Config")]
    [SerializeField] private FormConfigSO? _activeFormConfig;
    private readonly List<MarkInstance> _activeMarks = new();
    private readonly MarkManager? _markManager;
    private class MarkInstance
    {
        public Transform Marker;
        public float ExpiryTime;
    }
}
}