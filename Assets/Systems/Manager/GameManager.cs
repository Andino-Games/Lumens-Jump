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
        private const float MOVEMENT_INSTRUCTIONS_DURATION = 5f;
        private const float SHOW_TUTORIAL_DELAY = 0f;
        private const float REVIVE_THRESHOLD = 0.4f;

        private bool _isPaused;

        [SerializeField] private HudController hudController;
        [SerializeField] private PlayerDeath playerDeath;
        [SerializeField] private PlayerJump playerJump;
        [SerializeField] private RisingDeathZone deathZone;
        [SerializeField] private TutorialController tutorialController;
        [SerializeField] private MenuController menuController;

        //  Debugging
        [Header("Developer")]
        [SerializeField] private DeveloperUIController devUI;

        private void Awake()
        {
            if (devUI != null)
            {
                AdsManager.Instance.ConnectDeveloperUI(devUI);
            }

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

            tutorialController.HideTutorial();
            Invoke(nameof(ShowTutorial), SHOW_TUTORIAL_DELAY);
        }

        private void ShowTutorial()
        {
            tutorialController.ShowMovementInstruction();
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
            //  Clamp the score to 10 if the user hasn't completed the tutorial yet.
            if (tutorialController.IsActive && PersistentData.Instance.CurrentScore >= 10)
            {
                return;
            }

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
                int highscore = PersistentData.Instance.LoadHighScore();
                int currentScore = PersistentData.Instance.CurrentScore;

                if (currentScore >= highscore * REVIVE_THRESHOLD)
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

            //  Revive and continue playing
            Action onRewarded = () => 
            {
                AdsManager.Instance.RunGameplayTimer();
                ResumeGame();
                hudController.SetRevivePanelActive(false);
                playerJump.isFollowActive = true;
                playerDeath.ResetGame();

                //  Activate the death zone last
                deathZone.HandleResetZone();
            };

            Action onDismiss = async () =>
            {
                await GoGameOver(false);
            };

            AdsManager.Instance.ShowRewardedAd(onRewarded, onDismiss);

            Debug.Log("[GameManager] Show Revive Ad");
        }

        public async void SkipRevive()
        {
            await GoGameOver(false);
            AdsManager.Instance.AddReviveDismiss();
            
            Debug.Log("[GameManager] Skip Revive");
        }

        public void GoMainMenu()
        {
            menuController.ShowMainMenu();
        }

        public async Task GoGameOver(bool showAd = true) 
        {
            if (showAd == true || AdsManager.Instance.CanSkipAd() == false)
            {
                if (AdsManager.Instance.CanShowAd() == true)
                {
                    AdsManager.Instance.ShowInterstitialAd();
                }
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

        public void UploadHighscore(int score)
        {
            PersistentData.Instance.UploadScore(score);
        }
    }
}