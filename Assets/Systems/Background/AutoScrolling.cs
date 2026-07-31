using UnityEngine;

namespace Systems.Background
{

    [RequireComponent(typeof(Renderer))]
    public class AutoScrollComponent : MonoBehaviour
    {
        [Tooltip("This acceleration will increase the scroll speed over time.")]
        [SerializeField] private Vector2 scrollAcceleration = new Vector2(0, -0.01f);

        private Material _material;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            _material.mainTextureOffset += scrollAcceleration * Time.deltaTime;
        }
    }
}