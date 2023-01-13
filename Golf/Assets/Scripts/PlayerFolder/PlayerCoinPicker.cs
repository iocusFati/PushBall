using System;
using System.Collections.Generic;
using CoinFolder;
using Infrastructure.Services;
using Infrastructure.Services.Dispose;
using Infrastructure.Services.Spawners;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace PlayerFolder
{
    public class PlayerCoinPicker : IDefault
    {
        private readonly ICoinSpawner _coinSpawner;
        private readonly ICoinAnimationService _coinAnimation;
        private readonly IPlayerTrigger _playerTrigger;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ICoinParticle _coinParticle;
        
        private readonly List<Action> _onRaisedActions = new();

        public PlayerCoinPicker(ICoinSpawner coinSpawner, IGameStateMachine gameStateMachine,
            ICoinParticle coinParticle, IDefaultResetService defaultReset)
        {
            _coinSpawner = coinSpawner;
            _gameStateMachine = gameStateMachine;
            _coinParticle = coinParticle;
            
            defaultReset.AddListener(this);
        }

        public void AddListener(Action added) =>
            _onRaisedActions.Add(added);

        public void SetPlayerData(Player player) => 
            player.OnCoinTriggerEnter += Pick;

        public void ToDefault() => 
            _onRaisedActions.Clear();

        private void Pick(Collider coinColl)
        {
            GameObject coinGO = coinColl.gameObject;
            Coin coin = coinGO.GetComponent<Coin>();
            
            if (!coin.Triggered)
            {
                coin.Triggered = true;
                coin.Animator.Pick(coin, OnRaised);
            }
        }

        private void OnRaised(Coin coin)
        {
            _coinSpawner.ReleaseCoin(coin, out bool zeroCoinsLeft);
            _coinParticle.BurstCoins(coin.transform.position);

            foreach (var action in _onRaisedActions)
            {
                action.Invoke();
            }
            
            if (zeroCoinsLeft)
                _gameStateMachine.Enter<WinState>();
        }
    }
}