using System.Collections;
using Systems.Procedural;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


namespace Systems.Manager 
{
    public class GameManager : MonoBehaviour
    {
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pausePanel;
        private bool isPaused = false;
        

        private TextMeshProUGUI pointsText;
        private TextMeshProUGUI finalScoreText;
        private TextMeshProUGUI highScoreText;

        [SerializeField] private GameObject initialGround;

        private void Start()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            
            Time.timeScale = 1f;

            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == "GameScene")
            {
                pointsText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
            }
            else if (currentScene.name == "GameOverScene")
            {
                finalScoreText = GameObject.Find("FinalScoreText").GetComponent<TextMeshProUGUI>();
                highScoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
                ShowFinalScore();
            }
            else if (currentScene.name == "MainMenuScene")
            {
                PersistentData.Instance.LoadHighScore();
            }
        }

        private void Update()
        {

            if (pointsText != null)
            {
                pointsText.text = "Score: " + PersistentData.Instance.currentScore;
            }
        }
        

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f; 
            if (pausePanel != null) pausePanel.SetActive(true);
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f; // Reanuda el tiempo del juego.
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        

        public void AddPoints(int pointsToAdd)
        {
            PersistentData.Instance.currentScore += pointsToAdd;
        }

        public void GameOver()
        {
            PersistentData.Instance.SaveHighScore();
            SceneManager.LoadScene("GameOverScene");
        }

        public void ShowMainMenu()
        {
            
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenuScene");
            ResetGame();
        }

        private void ResetGame()
        {
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(0, 0, 0);
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }

            if (initialGround != null) initialGround.SetActive(true);

            LevelGenerator levelGen = FindObjectOfType<LevelGenerator>();
            if (levelGen != null) levelGen.ResetLevel();

            PersistentData.Instance.ResetScore();
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void ExitGame()
        {
            // Esto solo funciona en un build del juego, no en el editor.
            Application.Quit();
        }

        private void ShowFinalScore()
        {
            if (finalScoreText != null) finalScoreText.text = "Final Score: " + PersistentData.Instance.currentScore;
            if (highScoreText != null) highScoreText.text = "High Score: " + PersistentData.Instance.highScore;
        }
    }
}