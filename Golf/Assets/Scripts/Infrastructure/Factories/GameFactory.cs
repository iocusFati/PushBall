using System;
using Infrastructure.AssetProviderFolder;
using PlayerFolder;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assetProvider;
        private readonly PlayerCoinPicker _playerCoinPicker;

        public event Action<Player> OnPlayerCreated;

        public GameFactory(IAssets assets, PlayerCoinPicker playerCoinPicker)
        {
            _assetProvider = assets;
            _playerCoinPicker = playerCoinPicker;
        }

        public Player CreatePlayer(Vector3 at)
        {
            var player = _assetProvider.Instantiate<Player>(AssetPaths.PlayerPath, at);

            _playerCoinPicker.SetPlayerData(player);
            player.CameraFollower = Camera.main;
            player.CoinPicker = _playerCoinPicker;

            OnPlayerCreated.Invoke(player);
            return player;
        }
        
    }
}