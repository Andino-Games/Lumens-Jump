using Systems.Platforms;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

namespace Systems.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("GameOverZone"))
            {
                cinemachineCamera.Follow = null;
                Invoke(nameof(HandleGameOver), 1f);
            }
        }

        private void HandleGameOver()
        {
            gameManager.GameOver();
            //  SceneManager.LoadScene("GameOverScene");  // Carga la escena de Game Over
        }
    }
}