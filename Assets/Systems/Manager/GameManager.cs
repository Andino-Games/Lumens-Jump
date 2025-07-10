using Systems.UI;
using Systems.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Systems.Manager 
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isPaused;

        [SerializeField] private HudController hudController;
        [SerializeField] private GameObject initialGround;

        private void Start()
        {
            Time.timeScale = 1f;
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
            hudController.PausePanel();
        }

        private void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            hudController.ResumeGame();
        }
        
        public void AddPoints(int pointsToAdd)
        {
            PersistentData.Instance.currentScore += pointsToAdd;
            AudioManager.Instance.PlayUI("Score");
        }
        
        public void GameOver()
        {
            PersistentData.Instance.SaveHighScore();
            SceneManager.LoadScene("GameOverScene");
            AudioManager.Instance.PlaySFX("Dead", 1);
        }
    }
}