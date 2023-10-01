using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Factories.Game
{
    public class GameFactory : Factory, IGameFactory
    {
        private const string MainMenuHudPath = "Hud/MainMenu";
        private const string StartupHudPath = "Hud/StartupHud";
        private const string GameHudPath = "Hud/GameHud";
        private const string WinWindowPath = "Hud/WinWindow";
        private const string LoseWindowPath = "Hud/LoseWindow";

        private const string GameContextPath = "GameContext/GameContext";
        private const string MainPlayerPath = "MainPlayer/MainPlayer";
        private const string EnemyOnePath = "Enemies/Enemy(Zombi1)";
        
        private GameObject _gameHud;

        public GameFactory(IInstantiator instantiator) : base(instantiator) { }

        public GameObject MainPlayer { get; set; }
        public GameObject Enemy { get; set; }

        public GameObject CreateEnemy(Vector3 position)
        {
            GameObject enemyOne = InstantiateOnActiveScene(EnemyOnePath, position, Quaternion.identity, null);
            Enemy = enemyOne;
            return Enemy;
        }

        public GameObject CreateMainPlayer()
        {
            GameObject mainPlayer = InstantiateOnActiveScene(MainPlayerPath);
            MainPlayer = mainPlayer;
            return MainPlayer;
        }

        public GameObject CreateMainMenuHud()
        {
            GameObject mainMenuHud = InstantiateOnActiveScene(MainMenuHudPath);
            return mainMenuHud;
        }

        public GameObject CreateGameHud()
        {
            _gameHud = InstantiateOnActiveScene(GameHudPath);
            return _gameHud;
        }

        public GameObject CreateStartupHud()
        {
            GameObject startupHud = InstantiateOnActiveScene(StartupHudPath);
            return startupHud;
        }

        public GameObject CreateWinWindow()
        {
            GameObject winWindow = InstantiateOnActiveScene(WinWindowPath);
            return winWindow;
        }

        public GameObject CreateLooseWindow()
        {
            GameObject looseWindow = InstantiateOnActiveScene(LoseWindowPath);
            return looseWindow;
        }

        public GameObject CreateGameContext()
        {
            GameObject gameContext = InstantiateOnActiveScene(GameContextPath);
            return gameContext;
        }
    }
}