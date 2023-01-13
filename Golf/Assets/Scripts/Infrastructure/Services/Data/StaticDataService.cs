using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.UI.Data;
using Infrastructure.AssetProviderFolder;
using Infrastructure.Data;
using UI.Services;
using UI.Windows;
using UnityEngine;

namespace Infrastructure.Services.Data
{
    public class StaticDataService : IStaticDataService
    {
        private const string WindowStaticData = "UI/Windows/WindowStaticData";
        
        private Dictionary<WindowID,Window> _windowConfigs;

        public void LoadStaticData()
        {
            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowStaticData)
                .Configs
                .ToDictionary(x => x.WindowID, x => x.Prefab);
        }

        public Window ForWindow(WindowID windowID) =>
            _windowConfigs.TryGetValue(windowID, out Window window)
                ? window
                : null;

        public UIStaticData LoadUIStaticData() => 
            Resources.Load<UIStaticData>(AssetPaths.UIStaticData);

        public PlayerStaticData LoadPlayerStaticData() => 
            Resources.Load<PlayerStaticData>(AssetPaths.PlayerPlayerData);

        public CoinStaticData LoadCoinStaticData() => 
            Resources.Load<CoinStaticData>(AssetPaths.CoinData);
    }
}