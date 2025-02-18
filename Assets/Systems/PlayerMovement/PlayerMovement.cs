using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joystick Config")]
    public Joystick joystick;  // Referencia al joystick

    [Header("Movement Config")]
    public float speed = 5f;      // Velocidad del movimiento
    public float smoothSpeed = 0.1f; // Suavidad del Lerp (0.1 = muy suave, 1 = instantáneo)
    public float maxX = 4f;       // Límite de movimiento en X

    private Vector3 targetPosition; // Posición objetivo

    void Update()
    {
        // Obtiene la entrada del joystick en el eje X
        float moveX = joystick.Horizontal * speed * Time.deltaTime;

        // Calcula la nueva posición objetivo
        targetPosition = transform.position + new Vector3(moveX, 0, 0);

        // Limita la posición dentro de los bordes de la pantalla
        targetPosition.x = Mathf.Clamp(targetPosition.x, -maxX, maxX);

        // Aplica suavizado con Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}

