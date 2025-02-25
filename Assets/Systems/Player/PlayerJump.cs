using UnityEngine;

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public LayerMask groundLayer;
        public Transform groundCheckOrigin;
        public float groundCheckDistance = 0.6f;

        private Rigidbody2D rb;
        private bool isGrounded;
        private PlayerEffects playerEffects; // Referencia a PlayerEffects
        private PlayerAnimatorManager playerAnimator;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerEffects = GetComponent<PlayerEffects>(); // Obtiene la referencia al script
            playerAnimator = GetComponent<PlayerAnimatorManager>();
        }

        void Update()
        {
            if (rb.linearVelocity.y >= 0) return;

            Vector2 direction = Vector2.down;
            isGrounded = Physics2D.Raycast(groundCheckOrigin.position, direction, groundCheckDistance, groundLayer);

            if (isGrounded)
            {
                playerAnimator.animator.SetTrigger("Jump");
                Jump();
            }
        }

        void Jump()
        {
            playerEffects?.PlayJumpEffect(); // Activa el feedback del salto
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
        }
    }
}