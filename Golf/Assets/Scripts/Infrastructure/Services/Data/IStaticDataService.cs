using DefaultNamespace.UI.Data;
using Infrastructure.Data;
using UI.Services;
using UI.Windows;

namespace Infrastructure.Services.Data
{
    public interface IStaticDataService : IService
    {
        public PlayerStaticData LoadPlayerStaticData();
        public CoinStaticData LoadCoinStaticData();
        public void LoadStaticData();
        public Window ForWindow(WindowID windowID);
        public UIStaticData LoadUIStaticData();
    }
}