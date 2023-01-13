using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Infrastructure;
using Infrastructure.Data;
using UnityEngine;

namespace CoinFolder
{
    public class CoinAnimator
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly float _idleRotateByZ;
        private readonly float _idleRaiseDuration;
        private readonly float _maxIdleHeight;
        private readonly float _onPickedRotateByZ;
        private readonly float _maxOnPickedHeight;
        private readonly float _onPickedRaiseDuration;
        private readonly float _initialHeight;

        private bool _coinIsIdling;

        private Coroutine _raiseCoinCoroutine;
        private Coroutine _rotateCoinCoroutine;
        private Coroutine _downCoinCoroutine;
        private TweenerCore<Vector3,Vector3,VectorOptions> _raiseTween;

        public CoinAnimator(ICoroutineRunner coroutineRunner, CoinStaticData coinStaticData)
        {
            _coroutineRunner = coroutineRunner;
            
            _idleRaiseDuration = coinStaticData.IdleRaiseDuration;
            _idleRotateByZ = coinStaticData.IdleRotateByZ;
            _maxIdleHeight = coinStaticData.MaxIdleHeight;
            _initialHeight = coinStaticData.InitialHeight;
            
            _onPickedRaiseDuration = coinStaticData.onPickedRaiseDuration;
            _onPickedRotateByZ = coinStaticData.OnPickedRotateByZ;
            _maxOnPickedHeight = coinStaticData.MaxOnPickedHeight;
        }

        public void CoinIdle(Coin coin)
        {
            RaiseCoin(coin, _maxIdleHeight, _idleRaiseDuration, true);
            if (_rotateCoinCoroutine != null)
                _coroutineRunner.StopCoroutine(_rotateCoinCoroutine);
            
            _rotateCoinCoroutine = _coroutineRunner.StartCoroutine(RotateCoin(coin, _idleRotateByZ));
        }

        public void Pick(Coin coin, Action<Coin> onRaised)
        {
            StopAllCoinCoroutines();
            _raiseTween.Kill();
            
            RaiseCoin(coin, _maxOnPickedHeight, _onPickedRaiseDuration, false, onRaised);
            _rotateCoinCoroutine = _coroutineRunner.StartCoroutine(RotateCoin(coin, _onPickedRotateByZ));
        }

        private void RaiseCoin(Coin coin, float maxHeight, float duration, bool doDown, Action<Coin> onRaised = null)
        {
            _raiseTween = coin.transform.DOMoveY(maxHeight, duration).
                SetEase(Ease.Linear)
                .OnComplete(() => 
                {
                    if (doDown)
                        DownCoin(coin, duration, maxHeight, _initialHeight);
                    
                    if (onRaised != null)
                    {
                        onRaised(coin);
                        _coroutineRunner.StopCoroutine(_rotateCoinCoroutine);
                        
                        _raiseTween.Kill();
                    }
                });
        }

        private void DownCoin(Coin coin, float duration, float maxHeight, float initialHeight)
        {
            _raiseTween = coin.transform.DOMoveY(initialHeight, duration).OnComplete(() =>
            {
                RaiseCoin(coin, maxHeight, duration, true);
            });
        }

        private IEnumerator RotateCoin(Coin coin, float rotationAngle)
        {
            while (true)
            {
                coin.transform.Rotate(0, 0, rotationAngle * Time.deltaTime);
    
                yield return null;
            }
        }

        private void StopAllCoinCoroutines()
        {
            _coroutineRunner.StopCoroutine(_rotateCoinCoroutine);
        }
    }
}