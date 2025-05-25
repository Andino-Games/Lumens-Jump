using System.Collections;
using Systems.Procedural;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public int points = 0;
        private int highScore;

        public float initialDelay = 5f;

        private TextMeshProUGUI pointsText;
        private TextMeshProUGUI finalScoreText;
        private TextMeshProUGUI highScoreText;

        //  Ya no necesitamos estos paneles como variables,
        //  ya que están en escenas separadas
        //  private GameObject gameOverPanel;
        //  private GameObject mainMenuPanel;
        //  private GameObject creditsPanel;

        [SerializeField] private GameObject initialGround;

        private void Start()
        {
            LoadHighScore();

            //  Dependiendo de la escena activa, asignamos las referencias de la UI
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == "GameScene")
            {
                pointsText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
            }
            else if (currentScene.name == "GameOverScene")
            {
                finalScoreText = GameObject.Find("FinalScoreText").GetComponent<TextMeshProUGUI>();
                highScoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
            }
            //  No necesitamos buscar referencias en MainMenuScene o CreditsScene,
            //  ya que no se utilizan en este script

            //ShowMainMenu();  //  Esto ya no es necesario aquí
        }

        private void Update()
        {
            if (pointsText != null)
            {
                pointsText.text = "Score: " + points;
            }
        }

        public void AddPoints(int pointsToAdd)
        {
            points += pointsToAdd;
            if (pointsText != null)
            {
                pointsText.text = "Score: " + points;
            }
        }

        public void GameOver()
        {
            SceneManager.LoadScene("GameOverScene");  //  Cargamos la escena de Game Over
            //if (gameOverPanel != null) gameOverPanel.SetActive(true);
            //if (finalScoreText != null) finalScoreText.text = "Final Score: " + points;

            if (points > highScore)
            {
                highScore = points;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
            //if (highScoreText != null) highScoreText.text = "High Score: " + highScore;

            //StartCoroutine(ReturnToMainMenu());
        }

        public void ShowMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");  //  Cargamos la escena del Main Menu
            points = 0;
            //if (gameOverPanel != null) gameOverPanel.SetActive(false);
            //if (creditsPanel != null) creditsPanel.SetActive(false);
            //if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            //if (pointsText != null) pointsText.text = "Score: 0";

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
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void ShowCredits()
        {
            SceneManager.LoadScene("CreditsScene");
            //if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            //if (creditsPanel != null) creditsPanel.SetActive(true);
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

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private IEnumerator ReturnToMainMenu()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}