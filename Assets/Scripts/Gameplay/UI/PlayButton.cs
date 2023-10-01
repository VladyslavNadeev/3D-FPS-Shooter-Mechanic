using Infrastructure.Services.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayButton : MonoBehaviour
{
    private IStateMachine<IGameState> _stateMachine;
    private IPersistenceProgressService _progress;

    [Inject]
    public void Construct(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progress)
    {
        _progress = progress;
        _stateMachine = stateMachine;
    }

    public void StartPlay()
    {
        _stateMachine.Enter<LoadLevelState, string>("Level1");
        _progress.PlayerData.Progress.Enemies = 0;
    }
}