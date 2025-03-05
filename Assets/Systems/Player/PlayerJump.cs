using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public float maxJumpForce = 15f; // Límite máximo de fuerza de salto
        public LayerMask groundLayer;
        public LayerMask trampolineLayer;
        public Transform groundCheckOrigin;
        public float groundCheckDistance = 0.6f;

        [Header("Game Difficulty")]
        public float difficultyMultiplier = 1.02f; // Factor de aumento progresivo
        public float difficultyIncreaseRate = 5f; // Cada cuántos segundos aumenta la dificultad
        
        private float timeSinceStart;
        private float initialJumpForce;
        private float initialGravityScale;
        
        private RaycastHit2D hit2D;
        private Rigidbody2D rb;
        public bool isJumping;
        private bool isGrounded;
        private bool isTrampoline;
        private PlayerEffects playerEffects;
        private PlayerAnimatorManager playerAnimator;
        
        public CinemachineCamera playerCamera;
        public Transform cameraBounds;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerEffects = GetComponent<PlayerEffects>();
            playerAnimator = GetComponent<PlayerAnimatorManager>();
            
            // Guardar valores iniciales
            initialJumpForce = jumpForce;
            initialGravityScale = rb.gravityScale;
        }

        void Update()
        {
            timeSinceStart += Time.deltaTime;
            
            // Aumentar dificultad cada cierto tiempo
            if (timeSinceStart >= difficultyIncreaseRate) 
            {
                IncreaseDifficulty();
                timeSinceStart = 0; // Reinicia el contador
            }

            if (rb.linearVelocity.y >= 0) return;

            Vector2 direction = Vector2.down;
            isGrounded = Physics2D.Raycast(groundCheckOrigin.position, direction, groundCheckDistance, groundLayer);
            GetRaycastTrampoline();

            if (isGrounded)
            {
                isJumping = true;
                playerAnimator.animator.SetTrigger("Jump");
                Jump();
            }
            else
            {
                isJumping = false;
            }

            if (isTrampoline)
            {
                hit2D.collider.GetComponent<PowerUps.Trampoline>().RaycastPowerUp();
            }

            CameraFollowCheck();
        }

        private void GetRaycastTrampoline()
        {
            hit2D = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, groundCheckDistance, trampolineLayer);
            isTrampoline = hit2D.collider != null;
        }

        private void CameraFollowCheck()
        {
            if (isJumping)
            {
                cameraBounds.position = new Vector3(cameraBounds.position.x, transform.position.y, cameraBounds.position.z);
                playerCamera.Follow = transform;
            }
            else if (rb.linearVelocity.y < 0)
            {
                playerCamera.Follow = null;
            }
        }

        void Jump()
        {
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
            // Incrementa la fuerza de salto sin pasarse del máximo
            if (jumpForce < maxJumpForce)
            {
                jumpForce *= difficultyMultiplier;
            }
            
            // Aumenta la gravedad progresivamente
            rb.gravityScale *= difficultyMultiplier;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
        }
    }
}