using System;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsInterstitialController
{
    private LevelPlayInterstitialAd ad;

    public AdsInterstitialController(string interstitialKey)
    {
        ad = new LevelPlayInterstitialAd(interstitialKey);

        ad.OnAdClosed += info => LoadAd();

        ad.OnAdLoaded += info => Debug.Log("[Interstitial] Cargado");
        ad.OnAdLoadFailed += err => Debug.LogWarning($"[Interstitial] Error al cargar: {err}");
        ad.OnAdDisplayed += info => Debug.Log("[Interstitial] Mostrando");
        ad.OnAdDisplayFailed += (err, info) => Debug.LogWarning($"[Interstitial] Error al mostrar: {err}");

        // Opcionales:
        ad.OnAdClicked += info => Debug.Log("[Interstitial] Click");
        ad.OnAdInfoChanged += info => Debug.Log("[Interstitial] Info actualizada");

        LoadAd();
    }

    public void ShowAdd()
    {
        if (ad.IsAdReady() == false)
        {
            Debug.LogWarning("[Interstitial] El anuncio no está listo aún");
            LoadAd();

            return;
        }

        ad.ShowAd("Must");

        Debug.LogWarning("[Interstitial] Mostrando anuncio");
    }

    public void LoadAd() => ad.LoadAd();
}
