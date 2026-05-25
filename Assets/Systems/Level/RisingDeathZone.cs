using UnityEngine;
using Systems.Manager; // Para acceder a GameManager.Instance.GameOver()

namespace Systems.Level
{
    public class RisingDeathZone : MonoBehaviour
    {
        [Header("Rising Config")]
        [SerializeField] private float risingSpeed = 0.3f;        // Velocidad base de subida
        [SerializeField] private float maxRisingSpeed = 2f;        // Velocidad máxima
        [SerializeField] private float accelerationRate = 0.01f;   // Aceleración por segundo

        [Header("References")]
        [SerializeField] private Transform playerTransform;        // Referencia al jugador

        [Header("Safety")]
        [SerializeField] private float maxDistanceBelowPlayer = 8f; // Nunca más de X unidades debajo del jugador

        private float _currentRisingSpeed; // Velocidad de ascenso actual de la DeathZone

        void Start()
        {
            _currentRisingSpeed = risingSpeed; // Inicializar con la velocidad base
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform not assigned to RisingDeathZone.", this);
            }
        }

        void Update()
        {
            // Subir constantemente
            transform.position += Vector3.up * _currentRisingSpeed * Time.deltaTime;

            // Acelerar gradualmente
            _currentRisingSpeed = Mathf.Min(_currentRisingSpeed + accelerationRate * Time.deltaTime, maxRisingSpeed);

            // Seguridad: no subir más alto que el jugador menos un margen
            // Esto evita que la DeathZone suba por encima del jugador si este se queda quieto mucho tiempo
            if (playerTransform != null)
            {
                float maxY = playerTransform.position.y - maxDistanceBelowPlayer;
                if (transform.position.y > maxY)
                {
                    transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // El jugador ha tocado la DeathZone, activar Game Over
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
                else
                {
                    Debug.LogWarning("GameManager.Instance not found when player hit DeathZone.");
                }
            }
        }
    }
}