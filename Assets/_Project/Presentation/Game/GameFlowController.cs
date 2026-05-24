#nullable enable
using HalfEmpty.Application;
using HalfEmpty.Application.FSM;
using HalfEmpty.Application.Game;
using HalfEmpty.Infrastructure.Configs;
using HalfEmpty.Presentation;
using HalfEmpty.Presentation.Player;
using HalfEmpty.Presentation.Enemies;
using UnityEngine;
namespace HalfEmpty.Presentation.Game
{
/// <summary>
/// MonoBehaviour adapter that wires up the GameFlowSM state machine on Start().
/// </summary>
public class GameFlowController : MonoBehaviour
{
    [Header("State Machine")]
    [SerializeField] private GameFlowSM? _gameFlowSM;
    [Header("References")]
    [SerializeField] private PlayerController? _player;
    /// <summary>Public access to the GameFlowSM for external state transitions.</summary>
    public GameFlowSM? GameFlowSMRef => _gameFlowSM;
    private GameStateMachine? _stateMachine;
    private void Start()
    {
        // Construct the GameFlowSM POCO (it is not a component)
        _gameFlowSM = new GameFlowSM();
        // Build the game flow state machine
        _stateMachine = new GameStateMachine();
        _stateMachine.ChangeState(new MenuState());
        _gameFlowSM.Initialise(_stateMachine);
        // Start playing immediately for Main scene
        _stateMachine.ChangeState(new PlayingState());
    }
    /// <summary>Public accessor for other components to switch game states.</summary>
    public void ChangeState(IState newState) => _gameFlowSM?.ChangeState(newState);
}
}
