using System;

namespace Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        public void Initialize();
        public void LoadRewardedVideo();
        public void ShowRewardedVideo();
        bool AdIsReady { get; }
        event Action OnAdsShowComplete;
    }
}