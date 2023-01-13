using System;
using System.Collections.Generic;
using Infrastructure.Services.Ads;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace UI.Windows.Levels
{
    public class LevelsWindow : Window
    {
        public RewardedAdLevel RewardedAdLevel;
        
        [SerializeField] private List<LevelButton> _levelButtons;
        private IGameStateMachine _stateMachine;

        public void Construct(IGameStateMachine stateMachine, 
            IAdsService adsService, 
            LevelAnimation levelAnimation)
        {
            _stateMachine = stateMachine;
            RewardedAdLevel.Construct(adsService, levelAnimation, stateMachine);
            RewardedAdLevel.Initialize();
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            Debug.Log(_levelButtons.Count);
            foreach (var button in _levelButtons)
            {
                button.Button.onClick.AddListener(() => _stateMachine.Enter<LoadLevelState, string>(GetLevelName(button)));
                Debug.Log(GetLevelName(button));
            }
        }

        private string GetLevelName(LevelButton button) => 
            String.Concat("Level ", (int)button.Level);
    }
}