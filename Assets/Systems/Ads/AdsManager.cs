using System;
using Systems.Utils;
using Unity.Services.LevelPlay;
using UnityEditor.PackageManager;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    const float TIME_BETWEEN_ADS = 120f;

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

        LevelPlay.Init(adsConfiguration.AppKey);
    }

    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration obj)
    {
        Debug.Log("[Ads] SDK inicializado correctamente");

        rewarded = new AdsRewardedController(adsConfiguration.RewardedKey);
        interstitial = new AdsInterstitialController(adsConfiguration.InterstitialKey);
    }

    private void LevelPlay_OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError($"[Ads] Error de inicialización: {error.ErrorMessage}");
    }

    public void RunGameplayTimer(bool startOver = false)
    {
        if (startOver == true)
        {
            canOfferRevive = true;
            timerController.StartOver();

            return;
        }

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

        return result;
    }

    public void ShowRewardedAd()
    {
        rewarded.ShowAdd(null);
    }

    public void ShowInterstitialAd()
    {
        interstitial.ShowAdd();
    }

    public void ResetRevive() => canOfferRevive = true;
}
