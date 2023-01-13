using Infrastructure.Services;
using UI.HUD;

namespace UI.Services
{
    public interface IUIFactory : IService
    {
        public void CreateWinWindow();
        public void CreateLevelsWindow();
        public IHUDText CreateHUD();
    }
}