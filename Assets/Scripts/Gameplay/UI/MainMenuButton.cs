using Infrastructure.Services.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuButton : MonoBehaviour
{
    private IStateMachine<IGameState> _stateMachine;
    private IPersistenceProgressService _progress;

    [Inject]
    public void Construct(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progress)
    {
        _progress = progress;
        _stateMachine = stateMachine;
    }

    public void ToMainMenu()
    {
        _stateMachine.Enter<LoadLevelState, string>("MainMenu");
        _progress.PlayerData.Progress.Enemies = 0;
        _progress.PlayerData.Progress.LoseCount = 0;
        _progress.PlayerData.Progress.WinCount = 0;
    }
}