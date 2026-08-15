using System;
using Systems.Audio;
using Systems.Level;
using Systems.Player;
using Systems.UI;
using Systems.Utils;
using UnityEngine;
using UnityEngine.Rendering.UI;


namespace Systems.Manager 
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isPaused;

        [SerializeField] private HudController hudController;
        [SerializeField] private PlayerDeath playerDeath;
        [SerializeField] private PlayerJump playerJump;
        [SerializeField] private RisingDeathZone deathZone;
        [SerializeField] private TutorialController tutorialController;

        private void Start()
        {
            Time.timeScale = 1f;
            PersistentData.Instance.ResetScore();
            PersistentData.Instance.LoadHighScore();

            AdsManager.Instance.RunGameplayTimer();

            hudController.SetRevivePanelActive(false);
            tutorialController.HideTutorial();

            Invoke(nameof(ShowTutorial), 0.5f);
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
        
        public void GameOver()
        {
            playerJump.isFollowActive = false;

            hudController.SetPause(null);

            bool doOfferRevive = AdsManager.Instance.CanOfferRevive();

            if (doOfferRevive == true)
            {
                tutorialController.HideTutorial();
                hudController.SetRevivePanelActive(true);

                if (CameraManager.HasInstance)
                {
                    CameraManager.Instance.SetCamera("Default");
                }

                PostProcessingManager.Instance?.SetColorAdjustments(Color.white, 0.05f);
                PostProcessingManager.Instance?.SetVignetteIntensity(0.3f, 0.05f);

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
            //   Revive Lambda function, only called when rewarded by an ad
            Action onRewarded = () => 
            {
                ResumeGame();
                playerDeath.ResetGame();
                deathZone.HandleResetZone();
                hudController.SetRevivePanelActive(false);
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

        public void GoMainMenu()
        {
            SceneManager.Instance.LoadScene("MainMenuScene");
            ResumeGame();
        }

        private void ShowTutorial()
        {
            tutorialController.SetMovementActive(true);
        }
    }
}