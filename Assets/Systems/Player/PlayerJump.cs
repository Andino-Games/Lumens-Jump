using UnityEngine;
using MoreMountains.Feedbacks; // Importante para usar Feel

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public LayerMask groundLayer;
        public Transform groundCheckOrigin;
        public float groundCheckDistance = 0.6f;

        [Header("Feedbacks")] 
        public MMF_Player jumpFeedback; // Feedback del salto
        
        private Rigidbody2D rb;
        private bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (rb.linearVelocity.y >= 0) return;

            Vector2 direction = Vector2.down;
            isGrounded = Physics2D.Raycast(groundCheckOrigin.position, direction, groundCheckDistance, groundLayer);

            if (isGrounded)
            {
                Jump();
            }
        }

        void Jump()
        {
            jumpFeedback?.PlayFeedbacks();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
        }
    }
}