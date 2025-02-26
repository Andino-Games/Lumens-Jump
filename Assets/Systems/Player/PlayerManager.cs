using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("GameOverZone"))
            {
                
                cinemachineCamera.Follow = null; // Stop camera from following
                Invoke(nameof(GameOver), 1f); // Optional delay for effect
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over!");
            // Add game over UI, scene reload, etc.
        }
    }
}
