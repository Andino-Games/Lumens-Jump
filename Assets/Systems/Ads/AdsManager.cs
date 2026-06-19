using System;
using Systems.Utils;
using Unity.Services.LevelPlay;
using UnityEditor.PackageManager;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    const float TIME_BETWEEN_ADS = 1f;

    [SerializeField] GameplayTimerController timerController;
    [SerializeField] AdsConfiguration adsConfiguration;

    private bool canOfferRevive;
    
    private AdsRewardedController rewarded;
    private AdsInterstitialController interstitial;

    public Action OnOfferRevive;

    private void Start()
    {
        LevelPlay.OnInitFailed += LevelPlay_OnInitFailed;
        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;

        // LevelPlay.Init(adsConfiguration.AppKey);

        ResetRevive();
    }

    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration obj)
    {
        Debug.Log("[Ads] SDK initialized properly");

        // rewarded = new AdsRewardedController(adsConfiguration.RewardedKey);
        // interstitial = new AdsInterstitialController(adsConfiguration.InterstitialKey);
    }

    private void LevelPlay_OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError($"[Ads] Initialization error: {error.ErrorMessage}");
    }

    public void RunGameplayTimer(bool startOver = false)
    {
        timerController.SetIsRunning(true);
    }

    public void StopGameplayTimer()
    {
        timerController.SetIsRunning(false);
    }

    public bool CanOfferRevive()
    {
        bool result = false;

        if (canOfferRevive == true)
        {
            canOfferRevive = false;
            timerController.SetIsRunning(false);

            return true;
        }

        return result;
    }

    public bool CanShowAd()
    {
        bool result = false;

        if (timerController.CurrentTime >= TIME_BETWEEN_ADS)
        {
            result = true;
        }
        else
        {
            Debug.Log("[Ads] Can't show ad: time between ads not reached");
        }

        Debug.Log($"[Ads] Time between ads: {timerController.CurrentTime}");

        return result;
    }

    public void ShowRewardedAd(Action onRewarded)
    {
        rewarded.ShowAdd(onRewarded);
    }

    public void ShowInterstitialAd()
    {
        interstitial.ShowAdd();
        timerController.ResetTimer();
    }

    public void ResetRevive() 
    { 
        canOfferRevive = true;
        PlayerPrefs.SetInt("ShowRevive", 1);
    }
}
