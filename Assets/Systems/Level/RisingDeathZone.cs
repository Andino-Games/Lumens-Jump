using UnityEngine;
using Systems.Manager; // Para acceder a GameManager.Instance.GameOver()

namespace Systems.Level
{
    /// <summary>
    /// Hace que la DeathZone suba constantemente, obligando al jugador a ascender.
    /// Al ser padre de "Platform Destroyer Position", las plataformas viejas
    /// se destruyen automáticamente sin modificar PlatformDestroyer.
    /// </summary>
    public class RisingDeathZone : MonoBehaviour
    {
        [Header("Velocidad de Ascenso")]
        [Tooltip("Velocidad inicial de subida en unidades/segundo.")]
        [SerializeField] private float risingSpeed = 0.3f;

        [Tooltip("Velocidad máxima de subida.")]
        [SerializeField] private float maxRisingSpeed = 2.5f;

        [Tooltip("Aceleración: cuánto aumenta la velocidad por segundo.")]
        [SerializeField] private float acceleration = 0.015f;

        [Header("Seguridad")]
        [Tooltip("Distancia máxima que la DeathZone puede estar por debajo del jugador. " +
                 "Evita que suba demasiado rápido y mate injustamente.")]
        [SerializeField] private float maxDistanceBelowPlayer = 10f;

        [Tooltip("Referencia al Transform del jugador.")]
        [SerializeField] private Transform playerTransform;

        [Header("Inicio Retrasado")]
        [Tooltip("Segundos de gracia antes de que la zona comience a subir.")]
        [SerializeField] private float startDelay = 3f;

        private float _currentSpeed;
        private float _elapsedTime;
        private bool _isActive;

        /// <summary>
        /// Velocidad actual de ascenso. Otros sistemas pueden leerla para sincronizarse.
        /// </summary>
        public float CurrentSpeed => _currentSpeed;

        private void Start()
        {
            _currentSpeed = risingSpeed;
            _isActive = false; // No activa al inicio, espera el delay
            if (playerTransform == null)
            {
                Debug.LogError("Player Transform not assigned to RisingDeathZone.", this);
            }
        }

        private void Update()
        {
            // Periodo de gracia al inicio del nivel
            if (!_isActive)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime < startDelay) return;
                _isActive = true;
            }

            // Ascenso constante
            transform.position += Vector3.up * (_currentSpeed * Time.deltaTime);

            // Aceleración gradual
            _currentSpeed = Mathf.Min(_currentSpeed + acceleration * Time.deltaTime, maxRisingSpeed);

            // Seguridad: no superar al jugador más allá del margen permitido
            if (playerTransform)
            {
                float maxY = playerTransform.position.y - maxDistanceBelowPlayer;
                if (transform.position.y > maxY)
                {
                    transform.position = new Vector3(
                        transform.position.x, maxY, transform.position.z);
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

        /// <summary>
        /// Permite pausar/reanudar el ascenso (útil para PowerUps como Jetpack).
        /// </summary>
        public void SetActive(bool active) => _isActive = active;
    }
}