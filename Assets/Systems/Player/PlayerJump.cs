using Systems.Audio;
using Unity.Cinemachine;
using UnityEngine;
using Systems.Level; // Necesario para RisingDeathZone

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
        public Transform cameraBounds;

        [Header("Camera Rising")]
        [Tooltip("Referencia al RisingDeathZone para sincronizar la velocidad de ascenso.")]
        public RisingDeathZone risingDeathZone;

        [Tooltip("Offset de velocidad sobre la DeathZone. " +
                 "Si es 0, la cámara sube al mismo ritmo. " +
                 "Si es positivo, la cámara sube un poco más rápido que el piso.")]
        public float cameraSpeedOffset = 0.1f;

        private float _timeSinceStart;
        private Rigidbody2D _rb;
        private PlayerEffects _playerEffects;
        private float _cameraBoundsY; // Almacena la posición Y que cameraBounds debe seguir

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerEffects = GetComponent<PlayerEffects>();
            // Inicializar _cameraBoundsY con la posición actual de cameraBounds si existe, de lo contrario 0.
            _cameraBoundsY = cameraBounds ? cameraBounds.position.y : 0f;
        }

        void Update()
        {
            _timeSinceStart += Time.deltaTime;
            
            if (_timeSinceStart >= difficultyIncreaseRate) 
            {
                IncreaseDifficulty();
                _timeSinceStart = 0;
            }
            
            CameraFollowCheck();
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
            // --- Ascenso autónomo de cameraBounds ---
            if (cameraBounds)
            {
                // Velocidad base: sincronizada con la DeathZone + offset
                float risingSpeed = (risingDeathZone ? risingDeathZone.CurrentSpeed : 0f) 
                                    + cameraSpeedOffset;
                _cameraBoundsY += risingSpeed * Time.deltaTime;

                // Si el jugador sube más alto, la cámara lo sigue (comportamiento original)
                // Esto asegura que la cámara no se quede atrás si el jugador es muy rápido
                if (transform.position.y > _cameraBoundsY)
                {
                    _cameraBoundsY = transform.position.y;
                }

                // Aplicar la posición calculada a cameraBounds
                cameraBounds.position = new Vector3(
                    cameraBounds.position.x, _cameraBoundsY, cameraBounds.position.z);
            }

            // --- Cinemachine follow: sin cambios en lógica ---
            // Cinemachine sigue al jugador solo cuando sube, de lo contrario se queda en cameraBounds
            if (playerCamera)
            {
                if (_rb.linearVelocity.y >= 0)
                {
                    playerCamera.Follow = transform; // Sigue al jugador
                }
                else
                {
                    playerCamera.Follow = null; // Deja de seguir al jugador (se queda en la posición de cameraBounds)
                }
            }
        }
    }
}