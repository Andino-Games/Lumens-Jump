using UnityEngine;

namespace Systems.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Joystick Config")]
        public Joystick joystick;

        [Header("Movement Config")]
        public float speed;
        [Tooltip("The speed at which the player moves. Go from 0.1 to 1.")]
        [Range(0.1f, 1f)] public float smoothSpeed;
        [Tooltip("The maximum distance the player can move on the X axis.")]
        [Range(1, 4)] public float maxX;

        private Vector3 _targetPosition;
        private PlayerEffects _playerEffects;
        private SpriteRenderer _spriteRenderer;

        void Start()
        {
            GetComponent<Rigidbody2D>();
            _playerEffects = GetComponent<PlayerEffects>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            
            float moveX = joystick.Horizontal * speed * Time.deltaTime;
            _targetPosition = transform.position + new Vector3(moveX, 0, 0);

            Flip(moveX);

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, -maxX, maxX);
            transform.position = Vector3.Lerp(transform.position, _targetPosition, smoothSpeed);

            if (Mathf.Abs(moveX) > 0.05f)
            {
                _playerEffects?.PlayMoveEffect();
            }
        }

        private void Flip(float horizontalMovement)
        {
            if (horizontalMovement > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (horizontalMovement < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
}