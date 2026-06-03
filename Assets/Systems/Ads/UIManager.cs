using System;
using UnityEngine;
using UnityEngine.UI;
using Systems.Utils;


namespace Systems.Ads
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject conteinerRestart;
        [SerializeField] private Button botonRevivir;
        public event Action OnGameOver;

        private void OnEnable()
        {
            AdsManager.Instance.OnOfferRevive += ShowInterface;
        }

        private void OnDisable()
        {
            AdsManager.Instance.OnOfferRevive -= ShowInterface;
        }

        private void ShowInterface()
        {
            if (conteinerRestart != null)
            {
                conteinerRestart.SetActive(true);
            }

            if (botonRevivir != null)
            {
                botonRevivir.onClick.RemoveAllListeners();

                botonRevivir.onClick.AddListener(AlPresionarBotonRevivir);
                
            }
        }

        // 3. Esta es la función puente que se ejecutará al hacer clic
        private void AlPresionarBotonRevivir()
        {
            Debug.Log("[UI] Clic detectado en el botón de revivir. Conectando con AdsManager...");
            AdsManager.Instance.ShowRewardedAd(() => 
            {
                Debug.Log("[UI] ¡Anuncio visto con éxito! Reviviendo...");

                conteinerRestart.SetActive(false);
                
                AdsManager.Instance.RunGameplayTimer();
              
                OnGameOver?.Invoke();
            });
        }
    }
}