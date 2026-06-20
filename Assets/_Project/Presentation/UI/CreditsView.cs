  #nullable enable
  using UnityEngine;
  using UnityEngine.UI;
  using TMPro;
  using UnityEngine.SceneManagement;
  namespace HalfEmpty.Presentation.UI
  {
  /// <summary>
  /// Credits / end screen. Fully self-contained — creates its own Canvas at runtime
  /// if no suitable parent Canvas exists. Shows "Спасибо, что играли" + Restart button.
  /// </summary>
  public class CreditsView : MonoBehaviour
  {
      [Header("UI References (optional — auto-created if null)")]
      [SerializeField] private TextMeshProUGUI? _creditsText;
      [SerializeField] private Button? _restartButton;

      [Header("Text")]
      [SerializeField] private string _creditsMessage = "Спасибо, что играли";

      [Header("Timing")]
      [SerializeField] private float _autoRestartDelay = 0f;

      private Canvas? _ownCanvas;
      private bool _shown;

      private void Awake()
      {
          gameObject.SetActive(false);
      }

      private void Start()
      {
          RegisterRestartButtonListener();
      }

      private void RegisterRestartButtonListener()
      {
          if (_restartButton != null)
          {
              _restartButton.onClick.RemoveListener(OnRestartButton);
              _restartButton.onClick.AddListener(OnRestartButton);
          }
      }

      /// <summary>Show credits. Creates own Canvas if needed. Pauses the game.</summary>
      public void ShowCredits()
      {
          if (_shown) return;
          _shown = true;

          Time.timeScale = 0f;

          EnsureCanvas();
          EnsureUIElements();
          RegisterRestartButtonListener();

          if (_creditsText != null)
              _creditsText.text = _creditsMessage;

          gameObject.SetActive(true);
          Debug.LogWarning("[CreditsView] ShowCredits() complete — game paused, credits visible.");
      }

      private void EnsureCanvas()
      {
          // Try to reuse FadeCanvas (created by ScreenFader)
          var fadeCanvas = Object.FindFirstObjectByType<Canvas>();
          if (fadeCanvas != null && fadeCanvas.name == "FadeCanvas")
          {
              _ownCanvas = fadeCanvas;
              transform.SetParent(_ownCanvas.transform, false);
              return;
          }

          // Otherwise create our own Canvas
          var canvasGO = new GameObject("CreditsCanvas");
          _ownCanvas = canvasGO.AddComponent<Canvas>();
          _ownCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
          _ownCanvas.sortingOrder = 1000;
          canvasGO.AddComponent<CanvasScaler>();
          canvasGO.AddComponent<GraphicRaycaster>();
          transform.SetParent(canvasGO.transform, false);
      }

      private void EnsureUIElements()
      {
          // If no Text assigned, create one
          if (_creditsText == null)
          {
              var textGO = new GameObject("CreditsText");
              textGO.transform.SetParent(transform, false);
              _creditsText = textGO.AddComponent<TextMeshProUGUI>();
              _creditsText.text = _creditsMessage;
              _creditsText.fontSize = 48;
              _creditsText.alignment = TextAlignmentOptions.Center;
              var rt = textGO.GetComponent<RectTransform>();
              rt.anchorMin = Vector2.zero;
              rt.anchorMax = Vector2.one;
              rt.offsetMin = Vector2.zero;
              rt.offsetMax = Vector2.zero;
          }

          // If no Button assigned, create one
          if (_restartButton == null)
          {
              var btnGO = new GameObject("RestartButton");
              btnGO.transform.SetParent(transform, false);
              _restartButton = btnGO.AddComponent<Button>();
              var btnText = btnGO.AddComponent<TextMeshProUGUI>();
              btnText.text = "Заново";
              btnText.fontSize = 24;
              btnText.alignment = TextAlignmentOptions.Center;
              var rt = btnGO.GetComponent<RectTransform>();
              rt.anchorMin = new Vector2(0.5f, 0f);
              rt.anchorMax = new Vector2(0.5f, 0f);
              rt.pivot = new Vector2(0.5f, 0f);
              rt.anchoredPosition = new Vector2(0, 80);
              rt.sizeDelta = new Vector2(200, 60);
          }
      }

      private void OnRestartButton()
      {
          Time.timeScale = 1f;
          gameObject.SetActive(false);
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
  }
  }