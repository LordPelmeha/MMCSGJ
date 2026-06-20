using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HalfEmpty.Editor
{
  /// <summary>
  /// On scene load, guarantees the root Canvas has a Canvas component (ScreenSpaceOverlay)
  /// and that a fullscreen FogOfWar overlay exists with a valid sprite so it can actually render.
  /// </summary>
  [InitializeOnLoad]
  public static class FogOverlayDiagnostics
  {
    static FogOverlayDiagnostics()
    {
      EditorApplication.delayCall += RunDiagnostics;
    }

    private static void RunDiagnostics()
    {
      // 1. Find or create the root Canvas with a Canvas component
      Canvas rootCanvas = Object.FindObjectOfType<Canvas>();
      if (rootCanvas == null)
      {
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        Debug.Log("[FogOverlayDiagnostics] Created root Canvas.");
        rootCanvas = canvasGO.GetComponent<Canvas>();
      }
      else
      {
        Debug.Log($"[FogOverlayDiagnostics] Found Canvas: {rootCanvas.name}, renderMode={rootCanvas.renderMode}");
      }

      // 2. Check for existing FogOfWarOverlay
      var existing = rootCanvas.transform.Find("FogOfWarOverlay");
      if (existing != null)
      {
        var img = existing.GetComponent<Image>();
        if (img != null && img.sprite == null)
        {
          Debug.LogWarning("[FogOverlayDiagnostics] FogOfWarOverlay exists but Image has no sprite — fixing.");
          img.sprite = CreateWhiteSprite();
          img.color = new Color(0f, 0f, 0f, 0f);
        }
      }
      else
      {
        Debug.Log("[FogOverlayDiagnostics] No FogOfWarOverlay found under Canvas.");
      }

      // 3. Verify ScreenFader exists
      var screenFader = Object.FindObjectOfType<HalfEmpty.Presentation.UI.ScreenFader>();
      if (screenFader == null)
      {
        Debug.LogWarning("[FogOverlayDiagnostics] ScreenFader not found in scene!");
      }
      else
      {
        Debug.Log("[FogOverlayDiagnostics] ScreenFader found.");
      }
    }

    private static Sprite CreateWhiteSprite()
    {
      var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
      tex.filterMode = FilterMode.Point;
      var pixels = new Color32[]
      {
        new Color32(255,255,255,255),
        new Color32(255,255,255,255),
        new Color32(255,255,255,255),
        new Color32(255,255,255,255),
      };
      tex.SetPixels32(pixels);
      tex.Apply();
      return Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 16f);
    }
  }
}
