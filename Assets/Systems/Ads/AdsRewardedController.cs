using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class AdsRewardedController
{
    private bool wasRewardGranted;
    private LevelPlayRewardedAd ad;

    private Action onRewardGranted;
    private Action onAdDismissed;

    public AdsRewardedController(string rewardedKey)
    {
        wasRewardGranted = false;
        ad = new LevelPlayRewardedAd(rewardedKey);

        ad.OnAdRewarded += OnRewarded;
        ad.OnAdClosed += OnClosed;

        ad.OnAdLoaded += info => Debug.Log("[Rewarded] Loading");
        ad.OnAdDisplayed += info => Debug.Log("[Rewarded] Showing");

        ad.OnAdLoadFailed += err => Debug.LogWarning($"[Rewarded] Error loading: {err}");
        ad.OnAdDisplayFailed += (err, info) => onAdDismissed?.Invoke();
        
        // Optionals:
        ad.OnAdClicked += info => Debug.Log("[Rewarded] Click");
        ad.OnAdInfoChanged += info => Debug.Log("[Rewarded] Info updated");

        LoadAd();
    }

    public void ShowAdd(Action onReward, Action onDismiss)
    {
        if (ad.IsAdReady() == false)
        {
            Debug.LogWarning("[Rewarded] Ad is not ready yet");
            LoadAd();

            return;
        }

        wasRewardGranted = false;
        onAdDismissed = onDismiss;
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
        LoadAd();
        CheckRewardAfterDelay();
    }

    private async void CheckRewardAfterDelay()
    {
        await System.Threading.Tasks.Task.Delay(1500); // ventana de gracia
        if (wasRewardGranted == false)
        {
            Debug.Log("[Rewarded] Cerrado sin completar");
            onAdDismissed?.Invoke();
        }
        onAdDismissed = null;
        onRewardGranted = null;
    }

    public void LoadAd() => ad.LoadAd();
}
