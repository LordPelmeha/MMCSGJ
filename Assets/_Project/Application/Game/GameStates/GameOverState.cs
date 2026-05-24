 #nullable enable
 using HalfEmpty.Application.FSM;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 namespace HalfEmpty.Application.Game
 {
 /// <summary>
 /// Game-over state.
 /// </summary>
 public class GameOverState : IState
 {
     public void Enter()
     {
         Time.timeScale = 0f;
         var gameOverView = Object.FindFirstObjectByType<HalfEmpty.Presentation.UI.GameOverView>();
         if (gameOverView != null)
         {
             gameOverView.gameObject.SetActive(true);
         }
         else
         {
             Debug.LogWarning("[GameOverState] GameOverView not found in scene.");
         }
     }
     public void Update()
     {
         // GameOverView buttons handle scene transitions internally via their own UnityEvent/OnClick
     }
     public void FixedUpdate() { }
     public void Exit()
     {
         Time.timeScale = 1f;
         var gameOverView = Object.FindFirstObjectByType<HalfEmpty.Presentation.UI.GameOverView>();
         if (gameOverView != null) gameOverView.gameObject.SetActive(false);
     }
 }
 }