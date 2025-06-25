using Systems.Platforms;
using Unity.Cinemachine; // Asegúrate de que este 'using' está presente para Cinemachine
using UnityEngine;

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public float maxJumpForce = 15f;
        public LayerMask groundLayer;

        [Header("Game Difficulty")]
        public float difficultyMultiplier = 1.02f;
        public float difficultyIncreaseRate = 5f;

        [Header("Camera Config")]
        public CinemachineCamera playerCamera;
        public Transform cameraBounds;

        private float _timeSinceStart;
        private Rigidbody2D _rb;
        private PlayerEffects _playerEffects;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerEffects = GetComponent<PlayerEffects>();
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
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & groundLayer) != 0 && 
                _rb.linearVelocityY <= 0)
            {
                PlatformContact contact = collision.gameObject.GetComponent<PlatformContact>();
                contact?.GrantPoints();
                Jump();
            }
        }

        void Jump()
        {
            _playerEffects?.PlayJumpEffect();
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
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
            // Si la velocidad vertical es positiva  o cero, la cámara sigue al jugador.
            if (_rb.linearVelocity.y >= 0)
            {
                if (playerCamera)
                {
                    playerCamera.Follow = transform;
                }
                // También actualizamos la posición del "límite" inferior de la cámara mientras subimos.
                if (cameraBounds)
                {
                    cameraBounds.position = new Vector3(cameraBounds.position.x, transform.position.y, cameraBounds.position.z);
                }
            }
            // Si la velocidad es negativa (cayendo), la cámara deja de seguir al jugador.
            // Esto permite que el jugador caiga fuera de la pantalla y active el "GameOverZone".
            else
            {
                if (playerCamera)
                {
                    playerCamera.Follow = null;
                }
            }
        }
        
    }
}