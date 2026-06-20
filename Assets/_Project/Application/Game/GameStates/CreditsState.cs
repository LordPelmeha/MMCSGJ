  #nullable enable
  using HalfEmpty.Application.FSM;
  using UnityEngine;
  namespace HalfEmpty.Application.Game
  {
  /// <summary>
  /// Credits / end state. Pauses the game and shows the CreditsView with "Спасибо, что играли".
  /// </summary>
  public class CreditsState : IState
  {
      public void Enter()
      {
          Debug.LogWarning("[CreditsState] >>> ENTER <<<");
          Time.timeScale = 0f;
          var credits = FindCreditsViewInScene();
          Debug.LogWarning($"[CreditsState] FindCreditsViewInScene result: {(credits != null ? credits.name : "NULL")}");
          if (credits != null)
          {
              credits.gameObject.SetActive(true);
              credits.ShowCredits();
              Debug.LogWarning("[CreditsState] CreditsView activated and shown.");
          }
          else
          {
              Debug.LogWarning("[CreditsState] CreditsView not found in scene.");
          }
      }
      public void Update() { }
      public void FixedUpdate() { }
      public void Exit()
      {
          Time.timeScale = 1f;
          var credits = Object.FindFirstObjectByType<HalfEmpty.Presentation.UI.CreditsView>();
          if (credits != null) credits.gameObject.SetActive(false);
      }
      private static HalfEmpty.Presentation.UI.CreditsView? FindCreditsViewInScene()
      {
          var all = Object.FindObjectsOfType<HalfEmpty.Presentation.UI.CreditsView>(true);
          if (all != null && all.Length > 0)
              return all[0];
          return null;
      }
  }
  }