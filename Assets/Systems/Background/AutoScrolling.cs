using UnityEngine;

namespace Systems.Background
{

    [RequireComponent(typeof(Renderer))]
    public class AutoScrollComponent : MonoBehaviour
    {
        [Header("Configuración de Auto-Scroll")]
        [Tooltip("La velocidad inicial a la que se desplazará la textura.")]
        [SerializeField] private Vector2 initialScrollSpeed = new Vector2(0, -0.05f);

        [Tooltip("Cuánto aumenta la velocidad cada segundo. Usar valores pequeños.")]
        [SerializeField] private Vector2 scrollAcceleration = new Vector2(0, -0.01f);

        private Material _material;
        private Vector2 _currentScrollSpeed;

        private void Start()
        {
            // Gracias a [RequireComponent], podemos estar seguros de que hay un Renderer.
            _material = GetComponent<Renderer>().material;
            _currentScrollSpeed = initialScrollSpeed;
        }

        private void Update()
        {
            // Aumentamos la velocidad actual basándonos en la aceleración.
            _currentScrollSpeed += scrollAcceleration * Time.deltaTime;

            // Aplicamos esta velocidad creciente al offset de la textura.
            _material.mainTextureOffset += _currentScrollSpeed * Time.deltaTime;
        }
    }
}