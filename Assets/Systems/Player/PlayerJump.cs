using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public LayerMask groundLayer;
        public LayerMask trampolineLayer;
        public Transform groundCheckOrigin;
        public float groundCheckDistance = 0.6f;

        private RaycastHit2D hit2D;

        private Rigidbody2D rb;
        public bool isJumping;
        private bool isGrounded;
        private bool isTrampoline;
        private PlayerEffects playerEffects; // Referencia a PlayerEffects
        private PlayerAnimatorManager playerAnimator;
        
        public CinemachineCamera playerCamera;
        public Transform cameraBounds;

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
                hit2D.collider.GetComponent<PowerUps.Trampoline>()
                    .RaycastPowerUp();
            }

                CameraFollowCheck();
        }

        private void GetRaycastTrampoline()
        {
            hit2D = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, groundCheckDistance, trampolineLayer);

            if (hit2D.collider != null)
            {
                isTrampoline = true;
            }
            else
            {
                isTrampoline = false;
            }
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

        public void Trampoline(float impulseForce, bool powerState)
        {
            if (powerState) isJumping = true;
            else isJumping = false;

            rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
        }
    }
}