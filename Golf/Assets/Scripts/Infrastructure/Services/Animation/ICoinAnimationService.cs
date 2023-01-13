using System.Collections;
using UnityEngine;

namespace Infrastructure.Services
{
    public interface ICoinAnimationService : IService

    {
        public void CoinIdle(GameObject coin);
        public void CoinPick(GameObject coin);
    }
}