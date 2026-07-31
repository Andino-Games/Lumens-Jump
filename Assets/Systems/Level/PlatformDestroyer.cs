using UnityEngine;

namespace Systems.Level
{
    public class PlatformDestroyer : MonoBehaviour
    {
        [SerializeField] private Transform targetPosition;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody2D.MovePosition(Vector2.MoveTowards(
                _rigidbody2D.position,
                targetPosition.position,
                Time.deltaTime * 10f
            ));
        }
    }
}