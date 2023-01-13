using CoinFolder;
using UnityEngine;

namespace Infrastructure.Services.Spawners
{
    public interface ICoinSpawner : IService
    {
        public void SpawnCoins(out int coinNumber);
        public void ReleaseCoins();
        public void ReleaseCoin(Coin coin, out bool zeroCoinsLeft);
    }
}