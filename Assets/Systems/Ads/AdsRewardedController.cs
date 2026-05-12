using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class AdsRewardedController
{
    private LevelPlayRewardedAd ad;

    private Action onRewardGranted;

    public AdsRewardedController(string rewardedKey)
    {
        ad = new LevelPlayRewardedAd(rewardedKey);

        ad.OnAdRewarded += OnRewarded;
        ad.OnAdClosed += info => LoadAd();

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

        onRewardGranted?.Invoke();
        onRewardGranted = null;
    }

    public void LoadAd() => ad.LoadAd();
}
