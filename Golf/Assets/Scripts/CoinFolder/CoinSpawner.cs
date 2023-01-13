using System;
using System.Collections.Generic;
using DefaultNamespace;
using Infrastructure;
using Infrastructure.AssetProviderFolder;
using Infrastructure.Data;
using Infrastructure.Services.Dispose;
using Infrastructure.Services.Spawners;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace CoinFolder
{
    public class CoinSpawner : ICoinSpawner, IDefault
    {
        private readonly IAssets _assetProvider;
        private readonly CoinStaticData _coinStaticData;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly float _radius;
        private readonly float _rotationAngleX;
        
        private CoinAnimator _coinAnimator;
        
        private IObjectPool<Coin> _coinPool;
        private readonly List<Coin> _activeCoins = new();
        private GameObject[] _coinSpawn;

        private IObjectPool<Coin> CoinPool
        {
            get
            {
                if (_coinPool == null)
                {
                    _coinPool = new ObjectPool<Coin>(
                        SpawnCoin, 
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

                return _coinPool;
            }
        }

        public CoinSpawner(IAssets assetProvider, CoinStaticData coinStaticData, ICoroutineRunner coroutineRunner,
            IDefaultResetService defaultResetService)
        {
            _assetProvider = assetProvider;
            _coinStaticData = coinStaticData;
            _coroutineRunner = coroutineRunner;

            _radius = coinStaticData.Radius;
            _rotationAngleX = coinStaticData.RotationAngleX;
            
            defaultResetService.AddListener(this);
        }

        public void SpawnCoins(out int coinNum)
        {
            Vector3[] coinPositions = GetCoinSpawnPositions();
            coinNum = coinPositions.Length;

            foreach (var position in coinPositions)
            {
                Coin coin = CoinPool.Get();

                coin.Triggered = false;
                coin.Animator.CoinIdle(coin);
                _activeCoins.Add(coin);
                coin.transform.position = new Vector3(position.x, _radius, position.z);
            }
        }

        public void ReleaseCoin(Coin coin, out bool zeroCoinsLeft)
        {
            CoinPool.Release(coin);
            _activeCoins.Remove(coin);
            zeroCoinsLeft = _activeCoins.Count == 0;
        }

        public void ReleaseCoins()
        {
            foreach (var coin in _activeCoins)
            {
                ReleaseCoin(coin);
            }
        }

        public void ToDefault() => 
            _coinPool = null;

        private void ReleaseCoin(Coin coin)
        {
            CoinPool.Release(coin);
            _activeCoins.Remove(coin);
        }

        private Coin SpawnCoin()
        {
            GameObject coinGO = _assetProvider.Instantiate<GameObject>(AssetPaths.Coin);

            Transform coinTransform = coinGO.transform;
            coinTransform.rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            coinTransform.localScale = new Vector3(_radius, _radius, _radius);

            Coin coin = coinGO.GetComponent<Coin>();
            _coinAnimator = new CoinAnimator(_coroutineRunner, _coinStaticData);
            coin.Animator = _coinAnimator;
            
            return coin;
        }

        private Vector3[] GetCoinSpawnPositions()
        {
            _coinSpawn = GameObject.FindGameObjectsWithTag(TagHolder.CoinSpawn);
            Vector3[] coinSpawnPositions = new Vector3[_coinSpawn.Length];
            
            for (int i = 0; i < _coinSpawn.Length; i++)
            {
                coinSpawnPositions[i] = _coinSpawn[i].transform.position;
            }

            return coinSpawnPositions;
        }
    }
}