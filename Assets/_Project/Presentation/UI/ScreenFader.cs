  #nullable enable
  using UnityEngine;
  using UnityEngine.UI;
  using System.Collections;
  namespace HalfEmpty.Presentation.UI
  {
  /// <summary>
  /// ScreenFader — completely self-contained fullscreen fade overlay.
  ///
  /// Creates everything it needs at runtime:
  ///   - "FadeCanvas"  (Canvas + CanvasScaler + GraphicRaycaster, SortOrder=999)
  ///   - "ScreenFader" child (Image with auto-generated white sprite)
  ///
  /// No Canvas pre-setup, no Image pre-wiring required. Just drop this component
  /// on any GameObject in the scene and call FadeOut() / FadeIn().
  ///
  /// All fades use Time.unscaledDeltaTime so they work when Time.timeScale = 0.
  /// </summary>
  public class ScreenFader : MonoBehaviour
  {
      [Header("Fade Timings (seconds)")]
      [SerializeField] private float _fadeOutDuration = 1.5f;
      [SerializeField] private float _fadeInDuration  = 2.0f;

      [Header("Startup")]
      [SerializeField] private bool _startOpaque = true;

      private Image? _fadeImage;
      private Canvas? _rootCanvas;
      private Coroutine? _activeFade;
      private static bool _fadeCanvasCreated;

      private void Awake()
      {
          Debug.LogWarning("[ScreenFader] Awake() — setting up overlay");
          EnsureFadeCanvasExists();
          EnsureOverlayImageExists();
      }

      private void Start()
      {
          if (_startOpaque)
          {
              SetOpaque();
              Debug.LogWarning("[ScreenFader] Start() — _startOpaque=true, will fade in");
              StartCoroutine(BeginFadeInNextFrame());
          }
      }

      private IEnumerator BeginFadeInNextFrame()
      {
          yield return null;
          FadeIn(_fadeInDuration);
      }

      /// <summary>Create FadeCanvas with Canvas + CanvasScaler + GraphicRaycaster if not already present.</summary>
      private void EnsureFadeCanvasExists()
      {
          if (_fadeCanvasCreated)
          {
              // Find existing canvas
              _rootCanvas = Object.FindObjectOfType<Canvas>();
              if (_rootCanvas != null && _rootCanvas.name == "FadeCanvas")
              {
                  Debug.Log("[ScreenFader] Reusing existing FadeCanvas");
                  return;
              }
          }

          // Create FadeCanvas root
          var canvasGO = new GameObject("FadeCanvas");
          _rootCanvas = canvasGO.AddComponent<Canvas>();
          _rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
          _rootCanvas.sortingOrder = 999;

          canvasGO.AddComponent<CanvasScaler>();
          canvasGO.AddComponent<GraphicRaycaster>();

          // Prevent the canvas from being destroyed on scene reload
          Object.DontDestroyOnLoad(canvasGO);

          _fadeCanvasCreated = true;
          Debug.LogWarning("[ScreenFader] Created FadeCanvas (Canvas + CanvasScaler + GraphicRaycaster, SortOrder=999)");
      }

      /// <summary>Create the fullscreen Image under FadeCanvas with a white sprite.</summary>
      private void EnsureOverlayImageExists()
      {
          if (_rootCanvas == null)
          {
              Debug.LogError("[ScreenFader] No FadeCanvas — cannot create overlay!");
              return;
          }

          // Reuse existing ScreenFader child if present
          var existing = _rootCanvas.transform.Find("ScreenFader");
          if (existing != null)
          {
              _fadeImage = existing.GetComponent<Image>();
              if (_fadeImage != null && _fadeImage.sprite != null)
              {
                  Debug.Log("[ScreenFader] Reusing existing ScreenFader Image");
                  return;
              }
          }

          // Create new
          var go = new GameObject("ScreenFader");
          go.transform.SetParent(_rootCanvas.transform, false);

          _fadeImage = go.AddComponent<Image>();
          _fadeImage.sprite = CreateWhiteSprite();
          _fadeImage.color = new Color(0f, 0f, 0f, 0f);

          // Full-screen stretch
          var rt = go.GetComponent<RectTransform>();
          rt.anchorMin = Vector2.zero;
          rt.anchorMax = Vector2.one;
          rt.offsetMin = Vector2.zero;
          rt.offsetMax = Vector2.zero;

          go.transform.SetAsLastSibling();

          Debug.LogWarning("[ScreenFader] Created ScreenFader Image overlay (fullscreen, white sprite)");
      }

      /// <summary>Fade to black over duration. Calls onComplete when fully opaque.</summary>
      public void FadeOut(float duration, System.Action? onComplete = null)
      {
          if (_fadeImage == null)
          {
              Debug.LogError("[ScreenFader] FadeOut FAILED — _fadeImage is null!");
              return;
          }
          if (_activeFade != null) StopCoroutine(_activeFade);
          Debug.LogWarning($"[ScreenFader] FadeOut({duration:F1}s) started — current alpha={_fadeImage.color.a:F2}");
          _activeFade = StartCoroutine(FadeRoutine(1f, duration, onComplete));
      }

      /// <summary>Fade from current alpha to transparent over duration. Calls onComplete when clear.</summary>
      public void FadeIn(float duration, System.Action? onComplete = null)
      {
          if (_fadeImage == null)
          {
              Debug.LogError("[ScreenFader] FadeIn FAILED — _fadeImage is null!");
              return;
          }
          if (_activeFade != null) StopCoroutine(_activeFade);
          Debug.LogWarning($"[ScreenFader] FadeIn({duration:F1}s) started — current alpha={_fadeImage.color.a:F2}");
          _activeFade = StartCoroutine(FadeRoutine(0f, duration, onComplete));
      }

      private IEnumerator FadeRoutine(float targetAlpha, float duration, System.Action? onComplete)
      {
          if (_fadeImage == null) yield break;

          var startColor = _fadeImage.color;
          float elapsed = 0f;

          while (elapsed < duration)
          {
              elapsed += Time.unscaledDeltaTime;
              float t = Mathf.Clamp01(elapsed / duration);
              var c = startColor;
              c.a = Mathf.Lerp(startColor.a, targetAlpha, t);
              _fadeImage.color = c;
              yield return null;
          }

          var final = _fadeImage.color;
          final.a = targetAlpha;
          _fadeImage.color = final;

          Debug.LogWarning($"[ScreenFader] FadeRoutine complete — alpha={final.a:F2}");
          onComplete?.Invoke();
      }

      /// <summary>Instantly set to fully opaque (black).</summary>
      public void SetOpaque()
      {
          if (_fadeImage == null) return;
          var c = _fadeImage.color;
          c.a = 1f;
          _fadeImage.color = c;
          Debug.LogWarning("[ScreenFader] SetOpaque() — alpha=1.0");
      }

      /// <summary>Instantly set to fully transparent (clear).</summary>
      public void SetTransparent()
      {
          if (_fadeImage == null) return;
          var c = _fadeImage.color;
          c.a = 0f;
          _fadeImage.color = c;
      }

      private static Sprite CreateWhiteSprite()
      {
          const int size = 2;
          var tex = new Texture2D(size, size, UnityEngine.TextureFormat.RGBA32, false);
          tex.filterMode = UnityEngine.FilterMode.Point;
          var pixels = new Color32[size * size];
          for (int i = 0; i < pixels.Length; i++)
              pixels[i] = new Color32(255, 255, 255, 255);
          tex.SetPixels32(pixels);
          tex.Apply();
          return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 16f);
      }

      private void OnDestroy()
      {
          Debug.Log("[ScreenFader] OnDestroy()");
      }
  }
  }
