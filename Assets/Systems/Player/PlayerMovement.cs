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

        private Vector3 targetPosition;
        private Rigidbody2D rb;
        private PlayerEffects playerEffects; // Referencia a PlayerEffects
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerEffects = GetComponent<PlayerEffects>(); // Obtiene la referencia al script
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            float moveX = joystick.Horizontal * speed * Time.deltaTime;
            targetPosition = transform.position + new Vector3(moveX, 0, 0);

            Flip(moveX);

            // Clamp posición para evitar salir del nivel
            targetPosition.x = Mathf.Clamp(targetPosition.x, -maxX, maxX);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Si el jugador se está moviendo, activa feedback de movimiento
            if (Mathf.Abs(moveX) > 0.05f)
            {
                playerEffects?.PlayMoveEffect();
            }
        }

        private void Flip(float horizontalMovement)
        {
            if (horizontalMovement > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalMovement < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}