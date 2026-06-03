using System;
using Systems.Manager;
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
    [SerializeField] private bool testMode = true;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LevelPlay.OnInitFailed += LevelPlay_OnInitFailed;
        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;

        LevelPlay.Init(adsConfiguration.AppKey);

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

    public void RunGameplayTimer(bool startOver = false)
    {
        timerController.SetIsRunning(true);
    }

    public void StopGameplayTimer()
    {
        timerController.SetIsRunning(false);
    }

    public bool CanOfferRevive(bool activate)
    {
        bool rewardedReady = rewarded != null || testMode;
    
        Debug.Log($"[Ads] CanOfferRevive. activate={activate}, canOfferRevive={canOfferRevive}, rewardedReady={rewardedReady}");

        if (activate && canOfferRevive && rewardedReady)
        {
            canOfferRevive = false;
            timerController.SetIsRunning(false);
            OnOfferRevive?.Invoke();
            return true;
        }

        return false;
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
    // public void AlPresionarBotonRevivir()
    // {
    //     Debug.Log("[Ads] Jugadora presionó revivir. Intentando mostrar anuncio premiado...");
    //
    //     ShowRewardedAd(() => 
    //     {
    //         Debug.Log("[Ads] ¡Anuncio visto con éxito! Reviviendo al jugador...");
    //     
    //         RunGameplayTimer();
    //
    //     });
    // }

    public void ShowRewardedAd(Action onRewarded)
    {
        if (testMode)
        {
            Debug.Log("[Ads] TEST MODE: simulando anuncio visto");
            onRewarded?.Invoke(); // simula que el jugador vio el ad
            return;
        }

        if (rewarded != null)
        {
            rewarded.ShowAdd(onRewarded);
        }
        else
        {
            Debug.LogError("[Ads] rewarded es null, SDK no inicializado");
            GameManager.Instance.GameOver(false);
        }
    }

    public void ShowInterstitialAd()
    {
        if (interstitial != null)
        {
            Debug.Log("[Ads] Mostrando Anuncio Intersticial...");
            interstitial.ShowAdd();
            timerController.ResetTimer();
        }
        else
        {
            Debug.LogWarning("[Ads] No se pudo mostrar Intersticial porque el SDK no está listo o no hay internet.");
        }
    }

    public void ResetRevive() 
    { 
        canOfferRevive = true;
        PlayerPrefs.SetInt("ShowRevive", 1);
    }
}
