  #nullable enable
  using HalfEmpty.Application.FSM;
  using UnityEngine;
  using UnityEngine.SceneManagement;
  using HalfEmpty.Presentation.UI;
  namespace HalfEmpty.Application.Game
  {
  /// <summary>
  /// Game-over state. Pauses the game and shows the PauseMenuView in "Game Over" mode.
  /// </summary>
  public class GameOverState : IState
  {
      public void Enter()
      {
          Time.timeScale = 0f;
          var pauseMenu = Object.FindFirstObjectByType<PauseMenuView>();
          if (pauseMenu != null)
          {
              pauseMenu.ShowGameOver();
          }
          else
          {
              Debug.LogWarning("[GameOverState] PauseMenuView not found in scene.");
          }
      }
      public void Update() { }
      public void FixedUpdate() { }
      public void Exit()
      {
          Time.timeScale = 1f;
          var pauseMenu = Object.FindFirstObjectByType<PauseMenuView>();
          if (pauseMenu != null) pauseMenu.Hide();
      }
  }
  }