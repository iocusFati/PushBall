using CameraLogic;
using CoinFolder;
using Infrastructure.AssetProviderFolder;
using Infrastructure.Factories;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Animation;
using Infrastructure.Services.Data;
using Infrastructure.Services.Dispose;
using Infrastructure.Services.Input;
using Infrastructure.Services.Particle;
using Infrastructure.Services.Spawners;
using Infrastructure.States.Interfaces;
using PlayerFolder;
using UI.Services;
using UI.Windows.Levels;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "Initial";
        private const string MainSceneName = "Level 1";
        
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IUpdatableLoop _updatableLoop;
        private readonly ILateUpdatableLoop _lateUpdatableLoop;

        public BootstrapState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services,
            ICoroutineRunner coroutineRunner, IUpdatableLoop updatableLoop, ILateUpdatableLoop lateUpdatableLoop)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;
            _updatableLoop = updatableLoop;
            _lateUpdatableLoop = lateUpdatableLoop;

            RegisterServices(services);
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, OnLoaded);

            var gameFactory = _services.Single<IGameFactory>();
            var staticDataService = _services.Single<IStaticDataService>();
            
            var playerMovement = CreatePlayerMovement(gameFactory, staticDataService);

            CreateTrajectoryDrawer(gameFactory);
            CreatePlayerAim(gameFactory, playerMovement);
            CreateCameraFollowing(gameFactory, staticDataService);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        #region CreateMethods

        private void CreateTrajectoryDrawer(IGameFactory gameFactory)
        {
            var trajectoryDrawer = new TrajectoryDrawer(_updatableLoop, gameFactory);
        }

        private PlayerCoinPicker CreatePlayerCoinPicker() =>
            new PlayerCoinPicker(_services.Single<ICoinSpawner>(), _gameStateMachine, _services.Single<ICoinParticle>(),
                _services.Single<IDefaultResetService>());

        private PlayerMovement CreatePlayerMovement(IGameFactory gameFactory, IStaticDataService staticDataService)
        {
            PlayerMovement playerMovement = new PlayerMovement
                (gameFactory, _coroutineRunner, staticDataService.LoadPlayerStaticData(),
                    _services.Single<IPlayerAnimation>(), _services.Single<IPlayerParticle>());
            
            return playerMovement;
        }

        private void CreatePlayerAim(IGameFactory gameFactory, PlayerMovement playerMovement)
        {
            PlayerAim playerAim = new PlayerAim(
                _services.Single<IInputService>(), gameFactory, _updatableLoop, _coroutineRunner, playerMovement);
        }

        private void CreateCameraFollowing(IGameFactory gameFactory, IStaticDataService staticDataService)
        {
            CameraFollowing following = new CameraFollowing(
                gameFactory, staticDataService.LoadPlayerStaticData(), _lateUpdatableLoop);
        }

        #endregion

        private void RegisterServices(AllServices services)
        {
            var adsService = RegisterAdsService();

            var staticData = RegisterStaticDataService(services);
            var defaultReset = services.RegisterService<IDefaultResetService>(new DefaultResetService());
            var assets = services.RegisterService<IAssets>(new AssetProvider());

            services.RegisterService<ICoinSpawner>(
                new CoinSpawner(assets, staticData.LoadCoinStaticData(), _coroutineRunner, defaultReset));
            services.RegisterService<ICoinParticle>(new CoinParticle(assets));
            
            var gameFactory = services.RegisterService<IGameFactory>(
                new GameFactory(assets, CreatePlayerCoinPicker()));

            services.RegisterService<IInputService>(
                InputService());
            services.RegisterService<IPlayerAnimation>(
                new PlayerAnimation(gameFactory, staticData.LoadPlayerStaticData(), _coroutineRunner));

            services.RegisterService<IPlayerParticle>(
                new PlayerParticle(staticData.LoadPlayerStaticData(), assets, _coroutineRunner, defaultReset));

            var uiFactory = services.RegisterService<IUIFactory>(
                new UIFactory(assets, staticData, _gameStateMachine,
                defaultReset, adsService, new LevelAnimation(staticData.LoadUIStaticData(), _coroutineRunner)));
            
            services.RegisterService<IWindowService>(new WindowService(uiFactory));
        }

        private AdsService RegisterAdsService()
        {
            var adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterService<IAdsService>(adsService);

            return adsService;
        }

        private IStaticDataService RegisterStaticDataService(AllServices services)
        {
            var staticData = services.RegisterService<IStaticDataService>(new StaticDataService());
            staticData.LoadStaticData();
            return staticData;
        }
        
        private static IInputService InputService() =>
            Application.isEditor
                ? new StandaloneInputService()
                : new MobileInputService();
    }
}