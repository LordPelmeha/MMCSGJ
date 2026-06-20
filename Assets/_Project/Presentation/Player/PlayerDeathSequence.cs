  #nullable enable
  using HalfEmpty.Domain.Enums;
  using HalfEmpty.Infrastructure.Events;
  using HalfEmpty.Presentation.UI;
  using UnityEngine;
  using UnityEngine.SceneManagement;
  using System.Collections;
  namespace HalfEmpty.Presentation.Player
  {
  /// <summary>
  /// Player death sequence using ScreenFader.
  ///
  /// Subscribes to PlayerHealthView.OnDeath via direct event + event asset.
  /// On death: FadeOut → hold black → reload → FadeIn on new scene load.
  /// </summary>
  public class PlayerDeathSequence : MonoBehaviour
  {
      [Header("Fade Timings (seconds)")]
      [SerializeField] private float _fadeOutDuration   = 1.5f;
      [SerializeField] private float _holdBlackDuration = 0.5f;
      [SerializeField] private float _fadeInDuration    = 2.0f;

      [SerializeField] private bool _debugLog = true;

      private bool _isDead;
      private bool _fadeInStarted;
      private ScreenFader? _screenFader;

      private void OnEnable()
      {
          if (_debugLog) Debug.LogWarning("[PlayerDeathSequence] OnEnable()");
          TryDirectSubscription();
          TryEventAssetSubscription();
      }

      private void OnDisable()
      {
          TryEventAssetUnsubscription();
      }

      private void Start()
      {
          if (_debugLog) Debug.LogWarning("[PlayerDeathSequence] Start() — scene loaded");

          ResolveScreenFader();

          if (_screenFader == null)
          {
              Debug.LogWarning("[PlayerDeathSequence] ScreenFader NOT found in scene!");
              return;
          }

          if (!_fadeInStarted)
          {
              _fadeInStarted = true;
              _screenFader.SetOpaque();
              Debug.LogWarning("[PlayerDeathSequence] Starting fade-in from black");
              StartCoroutine(BeginFadeInNextFrame());
          }
      }

      private IEnumerator BeginFadeInNextFrame()
      {
          yield return null;
          _screenFader?.FadeIn(_fadeInDuration);
          Debug.Log($"[PlayerDeathSequence] FadeIn({_fadeInDuration}s) started");
      }

      private void ResolveScreenFader()
      {
          if (_screenFader != null) return;
          _screenFader = Object.FindFirstObjectByType<ScreenFader>();
          if (_screenFader == null)
              Debug.LogWarning("[PlayerDeathSequence] ScreenFader NOT found in scene!");
          else
              Debug.LogWarning($"[PlayerDeathSequence] Found ScreenFader: {_screenFader.name}");
      }

      private void TryDirectSubscription()
      {
          var hpView = GetComponent<HalfEmpty.Presentation.Player.PlayerHealthView>();
          if (hpView != null)
          {
              hpView.OnDeath += StartDeathSequence;
              Debug.Log("[PlayerDeathSequence] Subscribed directly to PlayerHealthView.OnDeath");
          }
          else
          {
              Debug.LogWarning("[PlayerDeathSequence] PlayerHealthView NOT found on Player!");
          }
      }

      private void TryEventAssetSubscription()
      {
          var evt = Resources.Load<VoidEventSO>("Configs/Events/OnPlayerDeath");
          if (evt != null)
          {
              evt.Register(StartDeathSequence);
              Debug.Log("[PlayerDeathSequence] Registered to OnPlayerDeath event asset");
          }
          else
          {
              Debug.LogWarning("[PlayerDeathSequence] OnPlayerDeath event asset NOT found!");
          }
      }

      private void TryEventAssetUnsubscription()
      {
          var evt = Resources.Load<VoidEventSO>("Configs/Events/OnPlayerDeath");
          if (evt != null)
              evt.Unregister(StartDeathSequence);
      }

      public void StartDeathSequence()
      {
          if (_isDead) return;
          _isDead = true;
          _fadeInStarted = false;

          Debug.LogWarning("[PlayerDeathSequence] >>> DEATH <<<");

          DisablePlayerControl();
          ResolveScreenFader();

          // FadeOut handles the gradual transition — no SetOpaque() here
          if (_screenFader != null)
          {
              _screenFader.FadeOut(_fadeOutDuration);
              Debug.LogWarning($"[PlayerDeathSequence] FadeOut({_fadeOutDuration}s) called");
          }

          StartCoroutine(DeathCoroutine());
      }

      private void DisablePlayerControl()
      {
          var ctrl = GetComponent<PlayerController>();
          if (ctrl != null) ctrl.enabled = false;
          var col = GetComponent<Collider2D>();
          if (col != null) col.enabled = false;
          var rb = GetComponent<Rigidbody2D>();
          if (rb != null)
          {
              rb.linearVelocity = Vector2.zero;
              rb.bodyType = RigidbodyType2D.Kinematic;
          }
      }

      private IEnumerator DeathCoroutine()
      {
          yield return new WaitForSecondsRealtime(_fadeOutDuration);
          yield return new WaitForSecondsRealtime(_holdBlackDuration);

          Debug.LogWarning("[PlayerDeathSequence] >>> RELOADING SCENE <<<");

          Time.timeScale = 1f;
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
  }
  }
