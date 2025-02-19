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

        void Update()
        {
            float moveX = joystick.Horizontal * speed * Time.deltaTime;
            
            targetPosition = transform.position + new Vector3(moveX, 0, 0);
            
            // Clamp position to avoid going out of bounds.
            targetPosition.x = Mathf.Clamp(targetPosition.x, -maxX, maxX);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}

