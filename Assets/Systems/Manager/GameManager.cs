using System;
using Systems.Audio;
using Systems.Level;
using Systems.Player;
using Systems.UI;
using Systems.Utils;
using UnityEngine;


namespace Systems.Manager 
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isPaused;

        [SerializeField] private HudController hudController;
        [SerializeField] private PlayerDeath playerDeath;
        [SerializeField] private PlayerJump playerJump;
        [SerializeField] private RisingDeathZone deathZone;

        private void Start()
        {
            Time.timeScale = 1f;
            PersistentData.Instance.ResetScore();
            PersistentData.Instance.LoadHighScore();

            AdsManager.Instance.RunGameplayTimer();

            hudController.SetRevivePanelActive(false);
        }
        
        public void TogglePause()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0f; 
        }

        private void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            hudController.SetRevivePanelActive(false);
        }
        
        public void AddPoints()
        {
            PersistentData.Instance.AddPoints();
            AudioManager.Instance.PlayUI("Score");
        }
        
        public void GameOver()
        {
            playerJump.isFollowActive = false;

            bool doOfferRevive = AdsManager.Instance.CanOfferRevive();

            if (doOfferRevive == true)
            {
                //PauseGame();
                hudController.SetRevivePanelActive(true);

                CameraManager.Instance?.SetCamera("Default");
                PostProcessingManager.Instance?.SetColorAdjustments(Color.white, 0.05f);
                PostProcessingManager.Instance?.SetVignetteIntensity(0.3f, 0.05f);

                //SceneManager.Instance.LoadScene("GameOverScene");

                Debug.Log("[GameManager] Offer revive");
            }
            else
            {
                if (AdsManager.Instance.CanShowAd() == true)
                {
                    AdsManager.Instance.ShowInterstitialAd();
                }

                PersistentData.Instance.SaveHighScore();
                SceneManager.Instance.LoadScene("GameOverScene");

                Debug.Log("[GameManager] Game Over Scene");
            }
        }

        public void ShowReviveAd()
        {
            Action onRewarded = () => 
            {
                ResumeGame();
                playerDeath.ResetGame();
                deathZone.HandleResetZone();
                playerJump.isFollowActive = true;
            };

            AdsManager.Instance.ShowRewardedAd(onRewarded);

            Debug.Log("[GameManager] Show Revive Ad");
        }

        public void SkipRevive()
        {
            SceneManager.Instance.LoadScene("GameOverScene");
            
            Debug.Log("[GameManager] Skip Revive");
        }
    }
}