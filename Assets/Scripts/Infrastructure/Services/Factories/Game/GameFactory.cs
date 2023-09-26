using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Factories.Game
{
    public class GameFactory : Factory, IGameFactory
    {
        private const string StartupHudPath = "Hud/StartupHud";
        private const string GameHudPath = "Hud/GameHud";
        private const string WinWindowPath = "Hud/WinWindow";
        private const string LooseWindowPath = "Hud/LooseWindow";
        
        private GameObject _gameHud;

        public GameFactory(IInstantiator instantiator) : base(instantiator) { }

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
            GameObject looseWindow = InstantiateOnActiveScene(LooseWindowPath);
            return looseWindow;
        }
    }
}