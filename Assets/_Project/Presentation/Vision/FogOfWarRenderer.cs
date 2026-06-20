  #nullable enable
  using UnityEngine;
  using UnityEngine.UI;
  namespace HalfEmpty.Presentation.Vision
  {
  /// <summary>
  /// FogOfWarRenderer — only creates its own overlay when NO ScreenFader exists in the scene.
  /// When ScreenFader is present (death/respawn fades), this component yields to it
  /// and does NOT create a competing overlay.
  ///
  /// If no ScreenFader is found, falls back to creating its own fullscreen overlay for vision/fog gameplay.
  /// </summary>
  public class FogOfWarRenderer : MonoBehaviour
  {
      [Header("Fog Control")]
      [SerializeField] private float _targetAlpha = 0f;
      [SerializeField] private float _fadeSpeed = 2f;

      private Image? _overlayImage;
      private Canvas? _rootCanvas;
      private bool _overlayCreatedThisInstance;

      private void Awake()
      {
          Debug.LogWarning("[FogOfWarRenderer] Awake()");
          EnsureOverlayExists();
      }

      private void Start()
      {
          SetTransparent();
      }

      private void LateUpdate()
      {
          FadeOverlay();
      }

      private void EnsureOverlayExists()
      {
          if (_overlayImage != null && _overlayCreatedThisInstance) return;

          // If ScreenFader exists in the scene, don't create a competing overlay
          var existingScreenFader = Object.FindFirstObjectByType<HalfEmpty.Presentation.UI.ScreenFader>();
          if (existingScreenFader != null)
          {
              Debug.LogWarning("[FogOfWarRenderer] ScreenFader found in scene — NOT creating competing overlay. ScreenFader handles all fullscreen darkening.");
              _overlayCreatedThisInstance = true;
              return;
          }

          // Destroy any leftover from previous instance
          if (_overlayImage != null && _overlayImage.gameObject != null)
              Object.Destroy(_overlayImage.gameObject);

          _rootCanvas = Object.FindObjectOfType<Canvas>();
          if (_rootCanvas == null)
          {
              Debug.LogError("[FogOfWarRenderer] No Canvas found!");
              return;
          }
          Debug.Log($"[FogOfWarRenderer] Found Canvas: {_rootCanvas.name}, creating overlay...");

          var go = new GameObject("FogOfWarOverlay");
          go.transform.SetParent(_rootCanvas.transform, false);

          _overlayImage = go.AddComponent<Image>();
          _overlayImage.sprite = CreateWhiteSprite();
          _overlayImage.color = new Color(0f, 0f, 0f, 0f);

          var rt = go.GetComponent<RectTransform>();
          rt.anchorMin = Vector2.zero;
          rt.anchorMax = Vector2.one;
          rt.offsetMin = Vector2.zero;
          rt.offsetMax = Vector2.zero;

          go.transform.SetAsLastSibling();
          Debug.Log($"[FogOfWarRenderer] Overlay created at sibling index {go.transform.GetSiblingIndex()}");

          _overlayCreatedThisInstance = true;
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

      private void FadeOverlay()
      {
          if (_overlayImage == null) return;
          Color c = _overlayImage.color;
          c.a = Mathf.MoveTowards(c.a, _targetAlpha, _fadeSpeed * Time.unscaledDeltaTime);
          _overlayImage.color = c;
      }

      public void SetFogAlpha(float alpha)
      {
          _targetAlpha = Mathf.Clamp01(alpha);
      }

      public void SetOpaque()
      {
          _targetAlpha = 1f;
          if (_overlayImage != null)
              _overlayImage.color = new Color(0f, 0f, 0f, 1f);
      }

      public void SetTransparent()
      {
          _targetAlpha = 0f;
          if (_overlayImage != null)
              _overlayImage.color = new Color(0f, 0f, 0f, 0f);
      }

      public float CurrentAlpha => _overlayImage != null ? _overlayImage.color.a : 0f;
  }
  }
