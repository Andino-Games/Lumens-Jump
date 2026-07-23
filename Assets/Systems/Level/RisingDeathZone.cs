using UnityEngine;
using Systems.Manager;
using System.Collections; // Para acceder a GameManager.Instance.GameOver()

namespace Systems.Level
{
    /// <summary>
    /// Hace que la DeathZone suba constantemente, obligando al jugador a ascender.
    /// Al ser padre de "Platform Destroyer Position", las plataformas viejas
    /// se destruyen automáticamente sin modificar PlatformDestroyer.
    /// </summary>
    public class RisingDeathZone : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        [Header("Velocidad de Ascenso")]
        [Tooltip("Velocidad inicial de subida en unidades/segundo.")]
        [SerializeField] private float risingSpeed = 0.3f;

        [Tooltip("Velocidad máxima de subida.")]
        [SerializeField] private float maxRisingSpeed = 2.5f;

        [Tooltip("Aceleración: cuánto aumenta la velocidad por segundo.")]
        [SerializeField] private float acceleration = 0.015f;

        [Header("Seguridad")]
        [Tooltip("Distancia de seguridad mínima que la DeathZone debe mantener por debajo del jugador. " +
                 "Evita que la zona suba demasiado rápido y alcance al jugador.")]
        [SerializeField] private float safetyDistance = 4f;

        [Tooltip("Distancia máxima permitida por debajo del jugador. " +
                 "Si el jugador sube más allá de esta distancia, la DeathZone es arrastrada hacia arriba " +
                 "para que no quede rezagada.")]
        [SerializeField] private float maxDistanceBelowPlayer = 12f;

        [Tooltip("Referencia al Transform del jugador.")]
        [SerializeField] private Transform playerTransform;

        [Header("Inicio Retrasado")]
        [Tooltip("Segundos de gracia antes de que la zona comience a subir.")]
        [SerializeField] private float startDelay = 3f;
        
        private float _currentSpeed;
        private float _elapsedTime;
        private bool _isActive;
        private bool _gameOverTriggered;

        /// <summary>
        /// Velocidad actual de ascenso. Otros sistemas pueden leerla para sincronizarse.
        /// </summary>
        public float CurrentSpeed => _currentSpeed;

        private void Start()
        {
            _currentSpeed = risingSpeed;
            _isActive = false; // No activa al inicio, espera el delay
            _gameOverTriggered = false;
            
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
                if (_collider == null)
                {
                    Debug.LogError("RisingDeathZone: No Collider2D found on this GameObject or assigned in inspector.", this);
                }
            }

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

            // 1. Ascenso autónomo constante tentativo
            float targetY = transform.position.y + (_currentSpeed * Time.deltaTime);

            // Aceleración gradual
            _currentSpeed = Mathf.Min(_currentSpeed + acceleration * Time.deltaTime, maxRisingSpeed);

            // 2. Ajustes basados en la posición del jugador
            if (playerTransform)
            {
                // Si el jugador subió mucho, arrastrar la DeathZone hacia arriba para no rezagarse
                float minRequiredY = playerTransform.position.y - maxDistanceBelowPlayer;
                if (targetY < minRequiredY)
                {
                    targetY = minRequiredY;
                }

                // Seguridad: no dejar que suba demasiado cerca del jugador (distancia mínima de seguridad)
                float maxAllowedY = playerTransform.position.y - safetyDistance;
                if (targetY > maxAllowedY)
                {
                    targetY = maxAllowedY;
                }
            }

            // 3. REGLA DE ORO: La DeathZone NUNCA puede bajar. Solo puede subir o quedarse quieta.
            if (targetY > transform.position.y)
            {
                transform.position = new Vector3(
                    transform.position.x, targetY, transform.position.z);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_gameOverTriggered) return;

            if (other.CompareTag("Player"))
            {
                _gameOverTriggered = true;
                SetActive(false);
                if (_collider != null)
                {
                    _collider.enabled = false;
                }
            }
        }

        /// <summary>
        /// Permite pausar/reanudar el ascenso (útil para PowerUps como Jetpack).
        /// </summary>
        public void SetActive(bool active) => _isActive = active;


        public void HandleResetZone()
        {
            StartCoroutine(ResetCorroutine());
        }

        private IEnumerator ResetCorroutine()
        {
            _gameOverTriggered = false;
            SetActive(true);

            yield return new WaitForSeconds(1f);

            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }
    }
}