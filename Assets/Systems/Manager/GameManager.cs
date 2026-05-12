using Systems.Audio;
using Systems.UI;
using Systems.Utils;
using UnityEngine;


namespace Systems.Manager 
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isPaused;

        [SerializeField] private HudController hudController;

        private void Start()
        {
            Time.timeScale = 1f;
            PersistentData.Instance.ResetScore();
            PersistentData.Instance.LoadHighScore();

            AdsManager.Instance.RunGameplayTimer(true);
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
        }
        
        public void AddPoints()
        {
            PersistentData.Instance.AddPoints();
            AudioManager.Instance.PlayUI("Score");
        }
        
        public void GameOver()
        {
            bool doOfferRevive = AdsManager.Instance.CanOfferRevive();

            if(doOfferRevive == true)
            {
                AdsManager.Instance.ShowRewardedAd();
            }
            else
            {
                PersistentData.Instance.SaveHighScore();
                SceneManager.Instance.LoadScene("GameOverScene");


            }
        }
    }
}