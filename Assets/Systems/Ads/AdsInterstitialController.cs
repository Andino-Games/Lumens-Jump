using System;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsInterstitialController
{
    private LevelPlayInterstitialAd ad;

    public AdsInterstitialController(string interstitialKey)
    {
        ad = new LevelPlayInterstitialAd(interstitialKey);

        ad.OnAdClosed += info =>
        {
            LoadAd();
            Debug.Log("Closing add");
        };

        ad.OnAdLoaded += info => Debug.Log("[Interstitial] Loading");
        ad.OnAdLoadFailed += err => Debug.LogWarning($"[Interstitial] Error loading: {err}");
        ad.OnAdDisplayed += info => Debug.Log("[Interstitial] Showing");
        ad.OnAdDisplayFailed += (err, info) => Debug.LogWarning($"[Interstitial] Error showing: {err}");

        // Opcionales:
        ad.OnAdClicked += info => Debug.Log("[Interstitial] Click");
        ad.OnAdInfoChanged += info => Debug.Log("[Interstitial] Info updated");

        LoadAd();
    }

    public void ShowAdd()
    {
        if (ad.IsAdReady() == false)
        {
            Debug.LogWarning("[Interstitial] Ad is not ready yet");
            LoadAd();

            return;
        }

        ad.ShowAd("Must");

        Debug.LogWarning("[Interstitial] Showing ad");
    }

    public void LoadAd() => ad.LoadAd();
}
