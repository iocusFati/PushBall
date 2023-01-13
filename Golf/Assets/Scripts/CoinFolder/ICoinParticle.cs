using Infrastructure.Services;
using UnityEngine;

namespace CoinFolder
{
    public interface ICoinParticle : IService
    {
        public void BurstCoins(Vector3 pos);
    }
}