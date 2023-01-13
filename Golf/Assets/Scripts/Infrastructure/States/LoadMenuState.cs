using Infrastructure.States.Interfaces;
using UI.Services;

namespace Infrastructure.States
{
    public class LoadMenuState : IPayloadedState<WindowID>
    {
        private const string MenuSceneName = "Menu";
        
        private readonly SceneLoader _sceneLoader;
        private readonly IWindowService _windowService;
        private readonly IUIFactory _uiFactory;

        private WindowID _loadWindow;

        public LoadMenuState(SceneLoader sceneLoader, IWindowService windowService, IUIFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
        }

        public void Enter(WindowID window)
        {
            _sceneLoader.Load(MenuSceneName, OnLoaded);
            _loadWindow = window;
        }

        private void OnLoaded()
        {
            _windowService.Open(_loadWindow);
        }

        public void Exit()
        {
            
        }
    }
}