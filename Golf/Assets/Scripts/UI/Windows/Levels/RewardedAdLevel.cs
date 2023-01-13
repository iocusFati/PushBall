using System;
using System.Collections.Generic;
using Infrastructure.Services.Ads;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace UI.Windows.Levels
{
    public class RewardedAdLevel : MonoBehaviour
    {
        public List<LevelButton> RewardedAdLevels;
        
        private IAdsService _adsService;
        private LevelAnimation _levelAnimation;
        private LevelButton _buttonClicked;
        private IGameStateMachine _stateMachine;

        public void Construct(IAdsService adsService, LevelAnimation levelAnimation, IGameStateMachine stateMachine)
        {
            _adsService = adsService;
            _levelAnimation = levelAnimation;
            _stateMachine = stateMachine;
            
            adsService.OnAdsShowComplete += GetReward; 
        }

        private void GetReward()
        {
            Debug.Log("GetReward");
            _buttonClicked.Button.onClick.RemoveAllListeners();
            _levelAnimation.Unlock(_buttonClicked.Button.image, 
                () => _stateMachine.Enter<LoadLevelState, string>(GetLevelName(_buttonClicked)));
        }

        public void Initialize()
        {
            _adsService.Initialize();
            foreach (var level in RewardedAdLevels)
            {
                level.Button.onClick.AddListener(() => ShowAd(level));
            }
        }

        private void ShowAd(LevelButton button)
        {
            _buttonClicked = button;
            if (_adsService.AdIsReady)
                _adsService.ShowRewardedVideo();
        }
        
        private string GetLevelName(LevelButton button) => 
            String.Concat("Level ", (int)button.Level);
    }
}