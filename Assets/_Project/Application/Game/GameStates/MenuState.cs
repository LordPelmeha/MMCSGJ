 #nullable enable
 using HalfEmpty.Application.FSM;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 namespace HalfEmpty.Application.Game
 {
 /// <summary>
 /// Main menu state.
 /// </summary>
 public class MenuState : IState
 {
     public void Enter()
     {
         Time.timeScale = 1f;
         // The Main Menu scene should be loaded; HUD must be hidden
         var hud = Object.FindFirstObjectByType<HalfEmpty.Presentation.UI.HUDView>();
         if (hud != null) hud.gameObject.SetActive(false);
     }
     public void Update() { }
     public void FixedUpdate() { }
     public void Exit() { }
 }
 }