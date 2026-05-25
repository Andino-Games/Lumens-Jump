using Systems.Audio;
using Unity.Cinemachine;
using UnityEngine;
using Systems.Manager; // Asegúrate de que este using esté presente

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        [Header("Jump Config")]
        public float jumpForce = 7f;
        public float maxJumpForce = 15f;
        public LayerMask groundLayer;
        public Transform groundCheck;
        public Animator animator;

        [Header("Game Difficulty")]
        public float difficultyMultiplier = 1.02f;
        public float difficultyIncreaseRate = 5f;

        [Header("Camera Config")]
        public CinemachineCamera playerCamera;
        public Transform cameraBounds; // Este será el Transform que moveremos

        [Header("Camera Rising")]
        [Tooltip("Velocidad mínima constante a la que la cámara sube, obligando al jugador a subir.")]
        public float cameraRisingSpeed = 0.5f;
        [Tooltip("Velocidad máxima de ascenso de la cámara.")]
        public float maxCameraRisingSpeed = 2f;
        [Tooltip("Aceleración del ascenso por segundo.")]
        public float cameraRisingAcceleration = 0.01f;

        private float _timeSinceStart;
        private Rigidbody2D _rb;
        private PlayerEffects _playerEffects;
        private float _currentCameraRisingSpeed; // Velocidad actual de ascenso autónomo de la cámara
        private float _highestPlayerY; // La posición Y más alta alcanzada por el jugador

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerEffects = GetComponent<PlayerEffects>();
            _currentCameraRisingSpeed = cameraRisingSpeed; // Inicializar con la velocidad base
            _highestPlayerY = transform.position.y; // Inicializar con la posición Y actual del jugador
        }

        void Update()
        {
            _timeSinceStart += Time.deltaTime;
            
            if (_timeSinceStart >= difficultyIncreaseRate) 
            {
                IncreaseDifficulty();
                _timeSinceStart = 0;
            }
            
            CameraFollowCheck(); // La lógica de seguimiento de cámara se mueve aquí

            // La detección de Game Over por caída la moveremos al RisingDeathZone
            // if (CameraManager.Instance != null && GameManager.Instance != null)
            // {
            //     if (transform.position.y < CameraManager.Instance.GetCameraY() - CameraManager.Instance.GetDeathOffset())
            //     {
            //         GameManager.Instance.GameOver();
            //     }
            // }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & groundLayer) != 0 && 
                _rb.linearVelocityY <= 0 && collision.transform.position.y <= groundCheck.position.y)
            {
                Jump();
            }
        }

        void Jump()
        {
            _playerEffects?.PlayJumpEffect();
            animator.SetTrigger(Jump1);
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            AudioManager.Instance.PlaySfx("Bounce", 1);
        }

        private void IncreaseDifficulty()
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce *= difficultyMultiplier;
            }
            _rb.gravityScale *= difficultyMultiplier;
        }
        
        private void CameraFollowCheck()
        {
            // Ascenso autónomo constante de cameraBounds
            if (cameraBounds)
            {
                // La cámara siempre sube a una velocidad mínima constante
                float autonomousRise = _currentCameraRisingSpeed * Time.deltaTime;
                float newBoundsY = cameraBounds.position.y + autonomousRise;

                // Acelerar gradualmente la velocidad de ascenso autónomo
                _currentCameraRisingSpeed = Mathf.Min(
                    _currentCameraRisingSpeed + cameraRisingAcceleration * Time.deltaTime,
                    maxCameraRisingSpeed);

                // Si el jugador sube más rápido, la cámara lo sigue (comportamiento original)
                // Actualizar la posición Y más alta del jugador
                if (transform.position.y > _highestPlayerY)
                {
                    _highestPlayerY = transform.position.y;
                }

                // Tomar el valor mayor: el ascenso autónomo o la posición más alta del jugador
                // Esto asegura que la cámara siempre suba, pero también siga al jugador si este la supera.
                newBoundsY = Mathf.Max(newBoundsY, _highestPlayerY);

                cameraBounds.position = new Vector3(
                    cameraBounds.position.x,
                    newBoundsY,
                    cameraBounds.position.z);
            }

            // Cámara de Cinemachine sigue al jugador solo cuando sube
            // Esto es para el 'Follow' de Cinemachine, no para el movimiento de cameraBounds
            if (playerCamera)
            {
                if (_rb.linearVelocity.y >= 0)
                {
                    playerCamera.Follow = transform; // Sigue al jugador
                }
                else
                {
                    playerCamera.Follow = null; // Deja de seguir al jugador (se queda en cameraBounds)
                }
            }
        }
    }
}
