using Infrastructure.AssetProviderFolder;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Data;
using Infrastructure.Services.Dispose;
using Infrastructure.States.Interfaces;
using UI.HUD;
using UI.Windows;
using UI.Windows.Levels;
using UnityEngine;

namespace UI.Services
{
    class UIFactory : IUIFactory
    {
        private readonly IAssets _assetProvider;
        private readonly WinWindow _winWindow;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IDefaultResetService _defaultReset;

        private Transform _uiRoot;
        private readonly LevelsWindow _levelsWindow;
        private readonly IAdsService _adsService;
        private readonly LevelAnimation _levelAnimation;

        public UIFactory(IAssets assetProvider, 
            IStaticDataService staticDataService, 
            IGameStateMachine gameStateMachine,
            IDefaultResetService defaultReset,
            IAdsService adsService, 
            LevelAnimation levelAnimation)
        {
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _defaultReset = defaultReset;
            _adsService = adsService;
            _levelAnimation = levelAnimation;

            _winWindow = (WinWindow)staticDataService.ForWindow(WindowID.Win);
            _levelsWindow = (LevelsWindow)staticDataService.ForWindow(WindowID.Levels);
        }

        public void CreateWinWindow()
        {
            if (_uiRoot == null) 
                CreateUIRoot();
            
            WinWindow window = Object.Instantiate(_winWindow, _uiRoot);
            window.Construct(_gameStateMachine, _defaultReset);
        }

        public void CreateLevelsWindow()
        {
            if (_uiRoot == null) 
                CreateUIRoot();

            LevelsWindow window = Object.Instantiate(_levelsWindow, _uiRoot);
            window.Construct(_gameStateMachine, _adsService, _levelAnimation);
        }

        public IHUDText CreateHUD()
        {
            IHUDText hud = _assetProvider.Instantiate<HUDText>(path.HUD, CreateHUDRoot());
            return hud;
        }

        private Transform CreateHUDRoot() => 
            _assetProvider.Instantiate<Transform>(AssetPaths.UIRoot);

        private void CreateUIRoot() => 
            _uiRoot = _assetProvider.Instantiate<Transform>(AssetPaths.UIRoot);
    }
}