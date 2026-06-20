  #nullable enable
  using HalfEmpty.Application.FSM;
  using HalfEmpty.Application;
  using UnityEngine;
  using UnityEngine.InputSystem;
  using HalfEmpty.Infrastructure.Events;
  using HalfEmpty.Presentation;
  using HalfEmpty.Presentation.Game;
  using HalfEmpty.Presentation.UI;
  namespace HalfEmpty.Application.Game
  {
  /// <summary>
  /// Active game-play state.
  /// </summary>
  public class PlayingState : IState
  {
      private GameFlowSM? _gameFlowSM;
      private VoidEventSO? _onGameFinish;
      public void Enter()
      {
          Time.timeScale = 1f;
          var controller = Object.FindFirstObjectByType<HalfEmpty.Presentation.Game.GameFlowController>();
          if (controller != null)
          {
              _gameFlowSM = controller.GameFlowSMRef;
          }
          _onGameFinish = Resources.Load<VoidEventSO>("Configs/Events/OnGameFinish");
          if (_onGameFinish != null)
          {
              _onGameFinish.Register(HandleGameFinish);
          }
          var hud = Object.FindFirstObjectByType<HUDView>();
          if (hud != null) hud.gameObject.SetActive(true);
      }
      public void Update()
      {
          if (Keyboard.current?.escapeKey.wasPressedThisFrame ?? false)
          {
              _gameFlowSM?.ChangeState(new PausedState());
          }
      }
      public void FixedUpdate() { }
      public void Exit()
      {
          Time.timeScale = 1f;
          _onGameFinish?.Unregister(HandleGameFinish);
          var hud = Object.FindFirstObjectByType<HUDView>();
          if (hud != null) hud.gameObject.SetActive(false);
      }
      private void HandleGameFinish()
      {
          _gameFlowSM?.ChangeState(new CreditsState());
      }
  }
}
