using System.Collections;
using Systems.Procedural;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        private TextMeshProUGUI pointsText;
        private TextMeshProUGUI finalScoreText;
        private TextMeshProUGUI highScoreText;

        [SerializeField] private GameObject initialGround;

        private void Start()
        {
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
                PersistentData.Instance.LoadHighScore(); //  Load High Score in MainMenu
            }
        }

        private void Update()
        {
            if (pointsText != null)
            {
                pointsText.text = "Score: " + PersistentData.Instance.currentScore;
            }
        }

        public void AddPoints(int pointsToAdd)
        {
            PersistentData.Instance.currentScore += pointsToAdd;
            if (pointsText != null)
            {
                pointsText.text = "Score: " + PersistentData.Instance.currentScore;
            }
        }

        public void GameOver()
        {
            PersistentData.Instance.SaveHighScore();
            SceneManager.LoadScene("GameOverScene");
        }

        public void ShowMainMenu()
        {
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

        public void ShowCredits()
        {
            SceneManager.LoadScene("CreditsScene");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private IEnumerator GroundStart()
        {
            yield return new WaitForSeconds(4f);
            if (initialGround != null) initialGround.SetActive(false);
        }

        private void ShowFinalScore()
        {
            if (finalScoreText != null) finalScoreText.text = "Final Score: " + PersistentData.Instance.currentScore;
            if (highScoreText != null) highScoreText.text = "High Score: " + PersistentData.Instance.highScore;
        }
    }
}