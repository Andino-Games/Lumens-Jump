using System;
using System.Xml.Serialization;
using Systems.Audio;
using Systems.Level;
using Systems.Player;
using Systems.UI;
using Systems.Utils;
using UnityEngine;
using UnityEngine.Rendering.UI;
using System.Threading.Tasks;


namespace Systems.Manager 
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isPaused;

        [SerializeField] private HudController hudController;
        [SerializeField] private PlayerDeath playerDeath;
        [SerializeField] private PlayerJump playerJump;
        [SerializeField] private RisingDeathZone deathZone;

        private void Awake()
        {
            hudController.OnReviveTimeEnded += async () => await GameOver();
            playerDeath.onGameOver += async () => await GameOver();
        }

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

            hudController.SetPause(_isPaused);
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
        }
        
        public void AddPoints()
        {
            PersistentData.Instance.AddPoints();
            AudioManager.Instance.PlayUI("Score");
        }
        
        public async Task GameOver()
        {
            playerJump.isFollowActive = false;

            hudController.SetPause(null);
            AdsManager.Instance.StopGameplayTimer();

            bool canOfferRevive = AdsManager.Instance.CanOfferRevive();

            if (canOfferRevive == true)
            {
                float reviveThreshold = 0.4f;
                int highscore = PersistentData.Instance.LoadHighScore();
                int currentScore = PersistentData.Instance.CurrentScore;

                if (currentScore >= highscore * reviveThreshold)
                {
                    OfferRevive();
                }
                else if (AdsManager.Instance.CanReplaceInterstitial())
                {
                    OfferRevive();
                }
                else
                {
                    await GoGameOver();
                }
            }
            else
            {
                await GoGameOver();
            }
        }

        public void ShowReviveAd()
        {
            hudController.SetRevivePanelActive(false);

            Action onRewarded = () => 
            {
                AdsManager.Instance.RunGameplayTimer();
                ResumeGame();
                playerDeath.ResetGame();
                deathZone.HandleResetZone();
                playerJump.isFollowActive = true;
                hudController.SetRevivePanelActive(false);
            };

            Action onDismiss = async () =>
            {
                await GoGameOver();
            };

            AdsManager.Instance.ShowRewardedAd(onRewarded, onDismiss);

            Debug.Log("[GameManager] Show Revive Ad");
        }

        public async void SkipRevive()
        {
            await GoGameOver();
            
            Debug.Log("[GameManager] Skip Revive");
        }

        public void GoMainMenu()
        {
            SceneManager.Instance.LoadScene("MainMenuScene");
            ResumeGame();
        }

        public async Task GoGameOver() 
        {
            if (AdsManager.Instance.CanShowAd() == true)
            {
                AdsManager.Instance.ShowInterstitialAd();
            }

            await PersistentData.Instance.SaveHighScore();
            SceneManager.Instance.LoadScene("GameOverScene");

            Debug.Log("[GameManager] Game Over Scene");
        }

        private void OfferRevive()
        {
            //  Show Revive Panel
            hudController.SetRevivePanelActive(true);

            if (CameraManager.HasInstance)
            {
                CameraManager.Instance.SetCamera("Default");
            }
            PostProcessingManager.Instance?.SetColorAdjustments(Color.white, 0.05f);
            PostProcessingManager.Instance?.SetVignetteIntensity(0.3f, 0.05f);

            Debug.Log("[GameManager] Offer revive");
        }
    }
}