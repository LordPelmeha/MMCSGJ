#nullable enable
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Infrastructure.Events;
using HalfEmpty.Domain.Enums;
using HalfEmpty.Presentation.Enemies;
using HalfEmpty.Infrastructure.Input;
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
    [SerializeField] private GameObject? _cursorMarkerPrefab;
    [Header("Config")]
    [SerializeField] private FormConfigSO? _activeFormConfig;
    private readonly List<MarkInstance> _activeMarks = new();
    private MarkManager? _markManager;
    private UnityInputProvider? _inputProvider;
    private class MarkInstance
    {
        public GameObject Marker;
        public MarkableView Markable;
        public float ExpiryTime;
    }
    private void Awake()
    {
        // Find MarkManager on self or in the scene
        _markManager = GetComponent<MarkManager>();
        if (_markManager == null)
            _markManager = FindObjectOfType<MarkManager>();
        // Find UnityInputProvider in the scene (may be on a different GameObject)
        _inputProvider = FindObjectOfType<UnityInputProvider>();
        if (_inputProvider == null)
            Debug.LogWarning("[MarkView] UnityInputProvider not found in scene!");
    }
    private void Update()
    {
        // Clean expired marks
        CleanExpiredMarks();
        // Handle mark input
        HandleMarkInput();
    }
    private void HandleMarkInput()
    {
        if (_activeFormConfig == null) 
        {
            Debug.Log("[MarkView] _activeFormConfig is NULL!");
            return;
        }
        if (_inputProvider == null) 
        {
            Debug.Log("[MarkView] _inputProvider is NULL!");
            return;
        }
        // Check input via Input System (fallback to legacy if needed)
        if (!_inputProvider.MarkPressed) return; // Right mouse button / F key
        Debug.Log("[MarkView] MarkPressed detected!");
        // Check max marks
        if (_activeMarks.Count >= _activeFormConfig.maxMarks)
        {
            Debug.LogWarning("[MarkView] Maximum marks reached.");
            return;
        }
        // Raycast to find Markable under cursor
        Vector2 worldPos = _inputProvider.MouseWorldPosition;
        Debug.Log($"[MarkView] MouseWorldPos={worldPos}");
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0.1f);
        if (hit.collider != null)
        {
            var markable = hit.collider.GetComponent<MarkableView>();
            if (markable != null && !markable.IsMarked)
            {
                PlaceMark(markable, hit.collider.gameObject);
            }
        }
    }
    private void PlaceMark(MarkableView markable, GameObject target)
    {
        // Instantiate cursor marker as child of the target
        GameObject markerObj = null;
        if (_cursorMarkerPrefab != null)
        {
            markerObj = Object.Instantiate(_cursorMarkerPrefab, target.transform);
            markerObj.name = "CursorMarker";
        }
        markable.ApplyMark();
        _activeMarks.Add(new MarkInstance
        {
            Marker = markerObj,
            Markable = markable,
            ExpiryTime = Time.time + _activeFormConfig.markDuration
        });
        _markManager?.RegisterMark();
    }
    private void CleanExpiredMarks()
    {
        if (_activeFormConfig == null) return;
        float now = Time.time;
        for (int i = _activeMarks.Count - 1; i >= 0; i--)
        {
            if (_activeMarks[i].ExpiryTime <= now)
            {
                // Remove the mark
                if (_activeMarks[i].Marker != null)
                    Object.Destroy(_activeMarks[i].Marker);
                if (_activeMarks[i].Markable != null)
                {
                    // Note: MarkableView has no Unmark method; isMarked stays true.
                    // This is intentional: mark is a one-time visual per target.
                }
                _markManager?.UnregisterMark();
                _activeMarks.RemoveAt(i);
            }
        }
    }
    /// <summary>Set the active form config for mark limits.</summary>
    public void SetActiveFormConfig(FormConfigSO? config)
    {
        _activeFormConfig = config;
        Debug.Log($"[MarkView] ActiveFormConfig set: {(_activeFormConfig != null ? "OK" : "NULL")}");
    }
    /// <summary>Expire all marks immediately (e.g. on form switch).</summary>
    public void ClearAllMarks()
    {
        foreach (var instance in _activeMarks)
        {
            if (instance.Marker != null)
                Object.Destroy(instance.Marker);
        }
        _activeMarks.Clear();
        if (_markManager != null)
        {
            while (_markManager.CurrentMarks > 0)
                _markManager.UnregisterMark();
        }
    }
}
}
