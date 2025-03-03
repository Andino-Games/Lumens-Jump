using System.Collections;
using TMPro;
using UnityEngine;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public int points = 0;
        private int highScore;

        public float initialDelay = 5f;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject creditsPanel;

        [Header("InitialGround")]
        [SerializeField] private GameObject initialGround;

        private void Start()
        {
            LoadHighScore();
            ShowMainMenu();
        }

        private void Update()
        {
            pointsText.text = points.ToString();
        }

        public void AddPoints(int pointsToAdd)
        {
            points += pointsToAdd;
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
            finalScoreText.text = "Final Score: " + points;
            
            if (points > highScore)
            {
                highScore = points;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
            highScoreText.text = "High Score: " + highScore;
        }

        public void ShowMainMenu()
        {
            points = 0;
            gameOverPanel.SetActive(false);
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        public void StartGame()
        {
            mainMenuPanel.SetActive(false);
            StartCoroutine(GroundStart());
        }

        public void ShowCredits()
        {
            mainMenuPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private IEnumerator GroundStart()
        {
            yield return new WaitForSeconds(initialDelay);
            initialGround.SetActive(false);
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
    }
}
