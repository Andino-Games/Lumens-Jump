using Systems.Platforms;
using Unity.Cinemachine;
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
        public bool isJumping;
        private bool isGrounded;
        private PlayerEffects playerEffects; // Referencia a PlayerEffects
        private PlayerAnimatorManager playerAnimator;
        
        public CinemachineCamera playerCamera;
        public Transform cameraBounds;
        
        [Header("Game Manager")]
        [SerializeField] private GameManager gameManager;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerEffects = GetComponent<PlayerEffects>(); // Obtiene la referencia al script
            playerAnimator = GetComponent<PlayerAnimatorManager>();

            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>();
            }
            else
            {
                return;
            }
        }

        void Update()
        {
            if (rb.linearVelocity.y >= 0) return;

            Vector2 direction = Vector2.down;
            isGrounded = Physics2D.Raycast(groundCheckOrigin.position, direction, groundCheckDistance, groundLayer);

            if (isGrounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(groundCheckOrigin.position, direction, groundCheckDistance, groundLayer);
                
                Platform platform = hit.collider.GetComponent<Platform>();

                if (platform != null)
                {
                    platform.PointCounter();    
                }
                else
                {
                    Debug.LogWarning("Platform could not be found");
                }
                
                
                isJumping = true;
                gameManager.AddPoints(1);
                Debug.Log(gameManager.points);
                playerAnimator.animator.SetTrigger("Jump");
                Jump();
            }
            else
            {
                isJumping = false;
            }

            CameraFollowCheck();
        }

        private void CameraFollowCheck()
        {
            if (isJumping)
            {
                
                cameraBounds.position = new Vector3(cameraBounds.position.x, transform.position.y, cameraBounds.position.z);
                playerCamera.Follow = transform; // Camera follows only when jumping
            }
            else if (rb.linearVelocityY < 0) // Stops following only when falling
            {
                cameraBounds.position = cameraBounds.position;
                playerCamera.Follow = null;
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