using UnityEngine;
using MoreMountains.Feedbacks; // Importante para usar Feel

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

        [Header("Feedbacks")] 
        public MMF_Player moveFeedback; // Para el movimiento lateral
        public MMF_Player jumpFeedback; // Para el salto

        private Vector3 targetPosition;
        private Rigidbody2D rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float moveX = joystick.Horizontal * speed * Time.deltaTime;
            targetPosition = transform.position + new Vector3(moveX, 0, 0);

            // Clamp posición para evitar salir del nivel
            targetPosition.x = Mathf.Clamp(targetPosition.x, -maxX, maxX);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Si el jugador se está moviendo, activa feedback de movimiento
            if (Mathf.Abs(moveX) > 0.05f)
            {
                moveFeedback?.PlayFeedbacks();
            }
        }

        public void Jump()
        {
            if (rb.linearVelocity.y < 0) // Solo salta si está cayendo
            {
                jumpFeedback?.PlayFeedbacks();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 7f); // Salto fuerte
            }
        }
    }
}