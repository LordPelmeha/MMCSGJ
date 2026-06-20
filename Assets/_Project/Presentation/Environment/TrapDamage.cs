  #nullable enable
  using HalfEmpty.Domain.Enums;
  using HalfEmpty.Infrastructure.Configs;
  using UnityEngine;
  using System.Collections;
  namespace HalfEmpty.Presentation.Environment
  {
  /// <summary>
  /// Applies damage to the player when they are inside a trap trigger zone.
  /// Damages once on enter, then repeats every _damageInterval seconds while the player stays inside.
  /// </summary>
  public class TrapDamage : MonoBehaviour
  {
      [Header("Damage")]
      [SerializeField] private float _damage = 10f;
      [Tooltip("Seconds between damage ticks while player is inside the trap.")]
      [SerializeField] private float _damageInterval = 1f;
      [Header("Config")]
      [SerializeField] private PlayerConfigSO? _playerConfig;
      [Header("Debug")]
      [SerializeField] private bool _debugLog = true;
      private bool _playerInside;
      private Coroutine? _damageCoroutine;
      private void OnTriggerEnter2D(Collider2D other)
      {
          if (!other.CompareTag("Player")) return;
          _playerInside = true;
          if (_debugLog) Debug.Log($"[TrapDamage] Player entered trap: {gameObject.name}");
          ApplyDamage();
          if (_damageCoroutine != null) StopCoroutine(_damageCoroutine);
          _damageCoroutine = StartCoroutine(DamageOverTime());
      }
      private void OnTriggerExit2D(Collider2D other)
      {
          if (!other.CompareTag("Player")) return;
          _playerInside = false;
          if (_debugLog) Debug.Log($"[TrapDamage] Player exited trap: {gameObject.name}");
          if (_damageCoroutine != null)
          {
              StopCoroutine(_damageCoroutine);
              _damageCoroutine = null;
          }
      }
      private void ApplyDamage()
      {
          var playerCtrl = FindFirstObjectByType<HalfEmpty.Presentation.Player.PlayerController>();
          if (playerCtrl == null) return;
          var form = playerCtrl.CurrentForm;
          var hpView = playerCtrl.GetComponent<HalfEmpty.Presentation.Player.PlayerHealthView>();
          if (hpView == null) return;
          bool died = hpView.TakeDamage(form, _damage);
          if (_debugLog) Debug.Log($"[TrapDamage] Damage applied! form={form} dmg={_damage} died={died}");
      }
      private IEnumerator DamageOverTime()
      {
          while (_playerInside)
          {
              yield return new WaitForSeconds(_damageInterval);
              if (_playerInside)
                  ApplyDamage();
          }
      }
      private void OnDrawGizmosSelected()
      {
          Gizmos.color = Color.red;
          var col = GetComponent<Collider2D>();
          if (col != null)
          {
              Gizmos.DrawWireSphere(transform.position, 0.5f);
          }
      }
  }
  }
