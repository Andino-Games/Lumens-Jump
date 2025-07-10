using Unity.Cinemachine;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump Config")]
        public float jumpForce = 7f;
        public float maxJumpForce = 15f;
        public LayerMask groundLayer;
        public Transform groundCheck;

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
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            AudioManager.Instance.PlaySFX("Bounce", 1);
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
            if (_rb.linearVelocity.y >= 0)
            {
                if (playerCamera)
                {
                    playerCamera.Follow = transform;
                }
                if (cameraBounds)
                {
                    cameraBounds.position = new Vector3(cameraBounds.position.x, transform.position.y, cameraBounds.position.z);
                }
            }
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