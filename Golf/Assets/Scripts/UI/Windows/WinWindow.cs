using CoinFolder;
using Infrastructure.Services.Dispose;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Windows
{
    class WinWindow : Window
    {
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _levelsButton;

        private CoinSpawner _coinSpawner;
        private IGameStateMachine _gameStateMachine;
        private IDefaultResetService _defaultReset;

        public void Construct(IGameStateMachine gameStateMachine, IDefaultResetService defaultReset)
        {
            _gameStateMachine = gameStateMachine;
            _defaultReset = defaultReset;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _replayButton.onClick.AddListener(() => 
                _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name));
            
            _levelsButton.onClick.AddListener(() =>
            {
                _defaultReset.ToDefault();
                _gameStateMachine.Enter<LoadMenuState, WindowID>(WindowID.Levels);
            });
        }
    }
}