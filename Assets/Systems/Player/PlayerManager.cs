using Systems.Platforms;
using UnityEngine;
using Unity.Cinemachine;

namespace Systems.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>(); // Encuentra el GameManager en la escena
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("GameOverZone"))
            {
                cinemachineCamera.Follow = null;
                Invoke(nameof(GameOver), 1f);
            }
        }

        private void GameOver()
        {
            gameManager.GameOver(); // Llama al Game Over del GameManager
        }
    }
}