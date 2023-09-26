using Infrastructure.Services.Factories.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.StateMachine.Game.States
{
    public class GameLoopState : IState, IGameState, IUpdatable
    {
        private readonly IGameFactory _gameFactory;
        private GameObject _gameHud;
        private GameObject _winWindow;
        private GameObject _looseWindow;
        private GameObject _respawnWindow;

        public GameLoopState(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            /*_gameHud = InitGameHud();
            InitializeGameHud(_gameHud);*/
        }

        private void InitializeGameHud(GameObject gameHud)
        {
            
        }

        private GameObject InitGameHud()
        {
            GameObject gameHud = _gameFactory.CreateGameHud();
            
            return gameHud;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}