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

        ad.OnAdLoaded += info => Debug.Log("[Rewarded] Cargado");
        ad.OnAdLoadFailed += err => Debug.LogWarning($"[Rewarded] Error al cargar: {err}");
        ad.OnAdDisplayed += info => Debug.Log("[Rewarded] Mostrando");
        ad.OnAdDisplayFailed += (err, info) => Debug.LogWarning($"[Rewarded] Error al mostrar: {err}");
        
        // Opcionales:
        ad.OnAdClicked += info => Debug.Log("[Rewarded] Click");
        ad.OnAdInfoChanged += info => Debug.Log("[Rewarded] Info actualizada");

        LoadAd();
    }

    public void ShowAdd(Action onReward)
    {
        if (ad.IsAdReady() == false)
        {
            Debug.LogWarning("[Rewarded] El anuncio no está listo aún");
            LoadAd();

            return;
        }

        onRewardGranted = onReward;
        ad.ShowAd("Revive");

        Debug.LogWarning("[Rewarded] Mostrando anuncio");
    }

    private void OnRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
    {
        Debug.Log($"[Rewarded] Recompensa: {reward.Name} x{reward.Amount}");

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
