using DefaultNamespace;
using Infrastructure.Factories;
using Infrastructure.Services.Input;
using Infrastructure.Services.Spawners;
using Infrastructure.States.Interfaces;
using PlayerFolder;
using UI.HUD;
using UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly ICoinSpawner _coinSpawner;
        private readonly IWindowService _windowService;
        private readonly IInputService _inputService;
        private readonly IUIFactory _uiFactory;

        private Vector3 _initialPoint;
        private IHUDText _hudText;
        private Player _player;

        public LoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            ICoinSpawner coinSpawner, IInputService inputService, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _coinSpawner = coinSpawner;
            _inputService = inputService;
            _uiFactory = uiFactory;
        }
        public void Enter(string sceneName)
        {
            if (sceneName == SceneManager.GetActiveScene().name)
                Reinitialize();
            else
                _sceneLoader.Load(sceneName, OnLoaded);
            
            _inputService.UnlockInput();
        }

        public void Exit() {}

        private void OnLoaded()
        {
            InitializeLevel();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitializeLevel()
        {
            _initialPoint = GameObject.FindGameObjectWithTag(TagHolder.InitialPointTag).transform.position;
            
            _coinSpawner.SpawnCoins(out int coinNumber);
            _hudText = _uiFactory.CreateHUD();
            _hudText.Initialize(coinNumber);
            
            _player = _gameFactory.CreatePlayer(_initialPoint);
            _player.CoinPicker.AddListener(() => _hudText.UpdateText());
        }

        private void Reinitialize()
        {
            _coinSpawner.SpawnCoins(out int coinNumber);
            _hudText.Initialize(coinNumber);
            _player.transform.position = _initialPoint;
        }
    }
}