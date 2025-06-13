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
        
        // --- SECCIÓN DE CÁMARA RE-AÑADIDA ---
        [Header("Camera Config")]
        public CinemachineCamera playerCamera;
        public Transform cameraBounds;
        // --- FIN DE SECCIÓN RE-AÑADIDA ---

        public bool isJumping;
        private float timeSinceStart;
        private Rigidbody2D rb;
        private PlayerEffects playerEffects;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerEffects = GetComponent<PlayerEffects>();
        }

        void Update()
        {
            timeSinceStart += Time.deltaTime;
            
            if (timeSinceStart >= difficultyIncreaseRate) 
            {
                IncreaseDifficulty();
                timeSinceStart = 0;
            }
            
            // --- LLAMADA AL MÉTODO RE-AÑADIDA ---
            // Llamamos a la lógica de la cámara en cada fotograma.
            CameraFollowCheck();
            // --- FIN DE LLAMADA ---
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            {
                PlatformContact contact = collision.gameObject.GetComponent<PlatformContact>();
                contact?.GrantPoints();
                Jump();
            }
        }
        
        void Jump()
        {
            isJumping = true;
            playerEffects?.PlayJumpEffect();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        public void Trampoline(float impulseForce, bool powerState)
        {
            isJumping = powerState;
            rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
        }

        private void IncreaseDifficulty()
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce *= difficultyMultiplier;
            }
            rb.gravityScale *= difficultyMultiplier;
        }
        
        
        private void CameraFollowCheck()
        {
            // Si la velocidad vertical es positiva (subiendo) o cero, la cámara sigue al jugador.
            if (rb.linearVelocity.y >= 0)
            {
                if (playerCamera != null)
                {
                    playerCamera.Follow = transform;
                }
                // También actualizamos la posición del "límite" inferior de la cámara mientras subimos.
                if (cameraBounds != null)
                {
                    cameraBounds.position = new Vector3(cameraBounds.position.x, transform.position.y, cameraBounds.position.z);
                }
            }
            // Si la velocidad es negativa (cayendo), la cámara deja de seguir al jugador.
            // Esto permite que el jugador caiga fuera de la pantalla y active el "GameOverZone".
            else
            {
                if (playerCamera != null)
                {
                    playerCamera.Follow = null;
                }
            }
        }
        
    }
}