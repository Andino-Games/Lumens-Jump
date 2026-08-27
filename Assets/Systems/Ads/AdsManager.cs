using System;
using Systems.Utils;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    //  Time unit is seconds.
    const float TIME_WITHOUT_ADS = 300f;
    const float TIME_BETWEEN_ADS = 120f;

    //  
    const int REPLACE_INTERSTITIAL_THRESHOLD = 2;
    const int FORCE_INTERSTITIAL_THRESHOLD = 2;

    [SerializeField] GameplayTimerController timerController;
    [SerializeField] AdsConfiguration adsConfiguration;

    private int interstitialCount;
    private int reviveDismissedCount;
    private bool canOfferRevive;
    private bool CanShowAds => PlayerPrefs.GetInt("CanShowAds") >= 100;

    private AdsRewardedController rewarded;
    private AdsInterstitialController interstitial;

    public Action OnOfferRevive;

    private void Start()
    {
        LevelPlay.OnInitFailed += LevelPlay_OnInitFailed;
        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;

        LevelPlay.Init(adsConfiguration.AppKey);

        LevelPlay.LaunchTestSuite();

        ResetRevive();
    }

    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration obj)
    {
        Debug.Log("[Ads] SDK initialized properly");

        rewarded = new AdsRewardedController(adsConfiguration.RewardedKey);
        interstitial = new AdsInterstitialController(adsConfiguration.InterstitialKey);
    }

    private void LevelPlay_OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError($"[Ads] Initialization error: {error.ErrorMessage}");
    }

    public void RunGameplayTimer()
    {
        timerController.SetIsRunning(true);
    }

    public void StopGameplayTimer()
    {
        timerController.SetIsRunning(false);

        if (CanShowAds == false)
        {
            int timeWithoutAdsSpent = (int)((timerController.CurrentTime / TIME_WITHOUT_ADS) * 100);

            PlayerPrefs.SetInt("CanShowAds", timeWithoutAdsSpent);

            Debug.Log($"Time without ads spent: {timeWithoutAdsSpent}");
        }
    }

    public bool CanOfferRevive()
    {
        if (CanShowAds == false)
        {
            return false;
        }

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
        if (CanShowAds == false)
        {
            return false;
        }

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

    public bool CanReplaceInterstitial()
    {
        if (interstitialCount >= REPLACE_INTERSTITIAL_THRESHOLD)
        {
            interstitialCount = 0;

            return true;
        }

        return false;
    }

    public bool CanSkipAd()
    {
        if (reviveDismissedCount >= FORCE_INTERSTITIAL_THRESHOLD)
        {
            reviveDismissedCount = 0;

            return false;
        }

        return true;
    }

    public void ShowRewardedAd(Action onRewarded, Action onDismiss)
    {
        rewarded.ShowAdd(onRewarded, onDismiss);
    }

    public void ShowInterstitialAd()
    {
        interstitial.ShowAdd();
        timerController.ResetTimer();
        interstitialCount++;
    }

    public void ResetRevive() => canOfferRevive = true;
    public void AddReviveDismiss() => reviveDismissedCount++;
}
