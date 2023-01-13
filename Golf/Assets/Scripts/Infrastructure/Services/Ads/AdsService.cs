using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameID = "5115815";
        private const string IOSGameID = "5115814";

        private const string AndroidRewardedVideoID = "Rewarded_Android";
        private const string IOSRewardedVideoID = "Rewarded_iOS";

        private string _gameID;
        private string _rewardedVideoID;
        
        public event Action OnAdsShowComplete;
        public bool AdIsReady { get; private set; }

        public AdsService()
        {
            SetIDs();
        }

        public void Initialize()
        {
            Advertisement.Initialize(_gameID, true, this);    
        }

        public void OnInitializationComplete() => 
            LoadRewardedVideo();

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => 
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");

        public void ShowRewardedVideo()
        {
            Advertisement.Show(_rewardedVideoID, this);
            AdIsReady = false;
            LoadRewardedVideo();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            OnAdsShowComplete.Invoke();
        }

        public void LoadRewardedVideo() => 
            Advertisement.Load(_rewardedVideoID, this);

        public void OnUnityAdsAdLoaded(string placementId)
        {
            AdIsReady = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        private void SetIDs()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameID = AndroidGameID;
                    _rewardedVideoID = AndroidRewardedVideoID;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameID = IOSGameID;
                    _rewardedVideoID = IOSRewardedVideoID;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameID = AndroidGameID;
                    _rewardedVideoID = AndroidRewardedVideoID;
                    break;
            }
        }
    }
}