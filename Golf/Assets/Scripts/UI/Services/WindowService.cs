namespace UI.Services
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowID windowID)
        {
            switch (windowID)
            {
                case WindowID.Unknown:
                    break;
                case WindowID.Win:
                    _uiFactory.CreateWinWindow();
                    break;
                case WindowID.Levels:
                    _uiFactory.CreateLevelsWindow();
                    break;
            }
        }
    }
}