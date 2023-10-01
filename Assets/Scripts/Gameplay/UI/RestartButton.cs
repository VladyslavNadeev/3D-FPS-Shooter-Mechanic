using Infrastructure.Services.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class RestartButton : MonoBehaviour
{
    private IStateMachine<IGameState> _stateMachine;
    private IPersistenceProgressService _progress;

    [Inject]
    public void Construct(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progress)
    {
        _progress = progress;
        _stateMachine = stateMachine;
    }

    public void Restart()
    {
        _stateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
        _progress.PlayerData.Progress.Enemies = 0;
    }
}