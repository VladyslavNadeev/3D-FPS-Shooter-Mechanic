using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.StaticData;
using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private IPersistenceProgressService _persistenceProgressService;
        private IStaticDataService _staticData;
        private List<Enemy> _enemies = new();

        public LoadLevelState(ISceneLoader sceneLoader, 
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory,
            IGameFactory gameFactory,
            IPersistenceProgressService persistenceProgressService,
            IStaticDataService staticData)
        {
            _staticData = staticData;
            _persistenceProgressService = persistenceProgressService;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
        }
        
        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();
        }

        private void InitGameWorld()
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                _uiFactory.CreateUiRoot();
                LevelStaticData levelData = _staticData.GetLevelDataFor(SceneManager.GetActiveScene().name);
                
                GameObject mainPlayer = _gameFactory.CreateMainPlayer();
                InitMainPlayer(mainPlayer);

                int countOfEnemies = 5;
                for (int i = 0; i < countOfEnemies; i++)
                {
                    Vector3 position = default;
                    foreach (var config in levelData.EnemySpawnConfigs)
                    {
                        position = new Vector3(
                            Random.Range(config.Range.x, -config.Range.x),
                            0f,
                            Random.Range(config.Range.z, -config.Range.z));
                    }
                    
                    _persistenceProgressService.PlayerData.Progress.AddEnemyCount(1);
                    GameObject enemy = _gameFactory.CreateEnemy(position);
                    Enemy enemyScript = InitEnemy(mainPlayer, enemy);
                    _enemies.Add(enemyScript);
                }

                GameObject gameHud = _gameFactory.CreateGameHud();
                InitGameHud(gameHud, mainPlayer);
                
                InitGameContext(mainPlayer, _enemies, gameHud);


                Init();

                _loadingCurtain.Hide();
            }
            else if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                GameObject mainMenuHud = _gameFactory.CreateMainMenuHud();
            }
            _loadingCurtain.Hide();
        }

        private void InitGameContext(GameObject mainPlayer, List<Enemy> enemies, GameObject gameHud)
        {
            GameObject gameContext = _gameFactory.CreateGameContext();
            gameContext.GetComponent<GameContext>().Init(
                _persistenceProgressService, 
                _gameFactory, 
                mainPlayer.GetComponent<PlayerVitality>(),
                mainPlayer.GetComponent<Player>(),
                mainPlayer.GetComponent<PlayerMovement>(),
                enemies,
                mainPlayer.GetComponentInChildren<LookCamera>(),
                gameHud);
        }

        private Enemy InitEnemy(GameObject mainPlayer, GameObject enemy)
        {
            enemy.GetComponent<Enemy>().Init(mainPlayer.GetComponent<PlayerVitality>(), _persistenceProgressService);
            return enemy.GetComponent<Enemy>();
        }

        private void InitGameHud(GameObject gameHud, GameObject mainPlayer)
        {
            gameHud.GetComponentInChildren<TextTutorial>().Init();
            gameHud.GetComponentsInChildren<Element>().ToList()
                .ForEach(x => x.Init(mainPlayer.GetComponentInChildren<PlayerBehaviour>()));
            gameHud.GetComponentInChildren<HitScreen>().Init(mainPlayer.GetComponent<PlayerVitality>());
            gameHud.GetComponentInChildren<HealthBar>().Init(mainPlayer.GetComponent<PlayerVitality>());
            gameHud.GetComponentInChildren<CountOfEnemies>().Init();
            gameHud.GetComponentInChildren<CountOfWins>().Init();
            gameHud.GetComponentInChildren<CountOfLoses>().Init();
        }

        private void InitMainPlayer(GameObject mainPlayer)
        {
            PlayerBehaviour playerBehaviour = mainPlayer.GetComponent<PlayerBehaviour>();
            
            mainPlayer.GetComponentInChildren<LookCamera>().Init(
                playerBehaviour,
                playerBehaviour.GetComponent<Rigidbody>());
            mainPlayer.GetComponentInChildren<Player>().Init();
            mainPlayer.GetComponentInChildren<PlayerMovement>().Init(
                playerBehaviour);
            mainPlayer.GetComponentsInChildren<WeaponAttachmentManager>().ToList().ForEach(x => x.Init());
            mainPlayer.GetComponentsInChildren<Weapon>().ToList().ForEach(x => x.Init(
                playerBehaviour));
            mainPlayer.GetComponentsInChildren<Muzzle>().ToList().ForEach(x => x.Init());
            mainPlayer.GetComponentsInChildren<CasingScript>().ToList().ForEach(x => x.Init());
            mainPlayer.GetComponentInChildren<CharacterAnimationEventHandler>().Init(playerBehaviour);
        }

        private void Init()
        {
            
        }
    }
}