using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.Factories.Game;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(ISceneLoader sceneLoader, 
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory,
            IGameFactory gameFactory)
        {
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
           _uiFactory.CreateUiRoot();

           Init();
        }

        private void Init()
        {
            
        }
    }
}