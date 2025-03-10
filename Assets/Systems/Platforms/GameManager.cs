using System.Collections;
using Systems.Procedural;
using UnityEngine;
using TMPro;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            LoadHighScore();
            ShowMainMenu();
        }

        private void Update()
        {
            pointsText.text = "Score: " + points; // Asegurar que siempre actualiza el puntaje en pantalla
        }

        public void AddPoints(int pointsToAdd)
        {
            points += pointsToAdd;
            pointsText.text = "Score: " + points; // Actualizar UI de puntaje en tiempo real
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
            pointsText.text = "Score: 0"; // Reiniciar la UI de puntaje
            
            ResetGame();
        }
        
        private void ResetGame()
        {
            // Reiniciar posición del jugador
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(0, 0, 0); // Ajusta la posición inicial correcta
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Asegurar que no tenga velocidad residual
            }

            // Hacer visible el suelo inicial
            if (initialGround != null)
            {
                initialGround.SetActive(true);
            }

            // Resetear LevelGenerator
            LevelGenerator levelGen = FindObjectOfType<LevelGenerator>();
            if (levelGen != null)
            {
                levelGen.ResetLevel();
            }
        }


        public void StartGame()
        {
            mainMenuPanel.SetActive(false);
            Debug.Log("StartGame llamado"); 
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
            yield return new WaitForSeconds(4f); 
            initialGround.SetActive(false);
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
    }
}
