using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class AdsRewardedController
{
    private bool wasRewardGranted;
    private LevelPlayRewardedAd ad;

    private Action onRewardGranted;

    public AdsRewardedController(string rewardedKey)
    {
        wasRewardGranted = false;
        ad = new LevelPlayRewardedAd(rewardedKey);

        ad.OnAdRewarded += OnRewarded;
        ad.OnAdClosed += OnClosed;

        ad.OnAdLoaded += info => Debug.Log("[Rewarded] Loading");
        ad.OnAdLoadFailed += err => Debug.LogWarning($"[Rewarded] Error loafing: {err}");
        ad.OnAdDisplayed += info => Debug.Log("[Rewarded] Showing");
        ad.OnAdDisplayFailed += (err, info) => Debug.LogWarning($"[Rewarded] Error showing: {err}");
        
        // Optionals:
        ad.OnAdClicked += info => Debug.Log("[Rewarded] Click");
        ad.OnAdInfoChanged += info => Debug.Log("[Rewarded] Info updated");

        LoadAd();
    }

    public void ShowAdd(Action onReward)
    {
        if (ad.IsAdReady() == false)
        {
            Debug.LogWarning("[Rewarded] Ad is not ready yet");
            LoadAd();

            return;
        }

        onRewardGranted = onReward;
        ad.ShowAd("Revive");

        Debug.LogWarning("[Rewarded] Showing ad");
    }

    private void OnRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
    {
        Debug.Log($"[Rewarded] Reward: {reward.Name} x{reward.Amount}");

        wasRewardGranted = true;

        onRewardGranted?.Invoke();
        onRewardGranted = null;
    }

    private void OnClosed(LevelPlayAdInfo info)
    {
        wasRewardGranted = false;

        LoadAd();
    }

    public void LoadAd() => ad.LoadAd();
}
