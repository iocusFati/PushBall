using Infrastructure.AssetProviderFolder;
using UnityEngine;
using UnityEngine.Pool;

namespace CoinFolder
{
    public class CoinParticle : ICoinParticle
    {
        private const string CoinPickParticle = "Coin/CoinPickParticle";

        private readonly IAssets _assetProvider;
        private IObjectPool<ParticleSystem> _coinPickParticles;

        private IObjectPool<ParticleSystem> CoinPick
        {
            get
            {
                if (_coinPickParticles == null)
                {
                    _coinPickParticles = new ObjectPool<ParticleSystem>(() =>
                            _assetProvider.Instantiate<ParticleSystem>(CoinPickParticle), 
                        coin =>
                        {
                            coin.gameObject.SetActive(true);
                        }, coin =>
                        {
                            coin.gameObject.SetActive(false);
                        }, coin => 
                        {
                            Object.Destroy(coin.gameObject);
                        });
                }

                return _coinPickParticles;
            }
        }

        public CoinParticle(IAssets assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void BurstCoins(Vector3 pos)
        {
            var burstParticle = CoinPick.Get();
            burstParticle.transform.position = pos;
            burstParticle.Play();
        }
    }
}