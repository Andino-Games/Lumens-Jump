using UnityEngine;

namespace Systems.Background
{

    [RequireComponent(typeof(Renderer))]
    public class AutoScrollComponent : MonoBehaviour
    {
        [Header("Auto-Scroll Configuration")]
        [Tooltip("Initial scroll speed of the texture.")]
        [SerializeField] private Vector2 initialScrollSpeed = new Vector2(0, -0.05f);
        
        [Tooltip("This acceleration will increase the scroll speed over time.")]
        [SerializeField] private Vector2 scrollAcceleration = new Vector2(0, -0.01f);

        private Material _material;
        private Vector2 _currentScrollSpeed;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _currentScrollSpeed = initialScrollSpeed;
        }

        private void Update()
        {
            _currentScrollSpeed += scrollAcceleration * Time.deltaTime;

            _material.mainTextureOffset += _currentScrollSpeed * Time.deltaTime;
        }
    }
}