using System;
using System.Collections;
using Infrastructure.AssetProviderFolder;
using Infrastructure.Data;
using Infrastructure.Services.Dispose;
using PlayerFolder;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Particle
{
    public class PlayerParticle : IPlayerParticle, IDefault
    {
        private readonly IAssets _assetProvider;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly float _particlePosOffset;
        private readonly float _particleCountKoeficient;
        private readonly float _particleDuration;

        private IObjectPool<ParticleSystem> _smokeParticlePool;

        private IObjectPool<ParticleSystem> SmokePool
        {
            get
            {
                if (_smokeParticlePool == null)
                {
                    _smokeParticlePool = new ObjectPool<ParticleSystem>(() =>
                        _assetProvider.Instantiate<ParticleSystem>(AssetPaths.PlayerSmoke), 
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

                return _smokeParticlePool;
            }
        }

        public PlayerParticle(PlayerStaticData playerStaticData, IAssets assetProvider,
            ICoroutineRunner coroutineRunner, IDefaultResetService defaultReset)
        {
            _assetProvider = assetProvider;
            _coroutineRunner = coroutineRunner;
            
            _particlePosOffset = playerStaticData.ParticlePosOffset;
            _particleDuration = playerStaticData.SmokeParticleDuration;
            _particleCountKoeficient = playerStaticData.ParticleCountKoeficient;
            
            defaultReset.AddListener(this);
        }

        public void SmokeBurst(Vector3 collisionPoint, Vector3 collisionNormal, float bounceForce)
        {
            var smokeParticle = SmokePool.Get();
            ParticleSystem.EmissionModule smokeEmission = smokeParticle.emission;
            
            smokeEmission.SetBurst(0, new ParticleSystem.Burst(
                0, Mathf.CeilToInt(bounceForce * _particleCountKoeficient)));
            smokeParticle.transform.position = collisionPoint + collisionNormal * _particlePosOffset;
            smokeParticle.transform.LookAt(collisionPoint);
            smokeParticle.Play();

            _coroutineRunner.StartCoroutine(smokeParticle.WaitForFinish(_particleDuration,
                () => SmokePool.Release(smokeParticle)));
        }

        public void ToDefault() => 
            _smokeParticlePool = null;
    }
}