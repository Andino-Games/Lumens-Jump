using Systems.UI.MouseClick;
using UnityEngine;

namespace Systems.UI.Cordel
{
    public class CordelHandler : MonoBehaviour, IClickDown, IClickUp
    {
        [SerializeField] private float clampRange;
        [SerializeField] private float dragSpeed = 1f;
        [SerializeField] private bool useXAxis = true;
        [SerializeField] private bool useYAxis = true;
        private bool _isDragging;
        private Rigidbody2D _rigidbody;
        private Camera _mainCamera;
        private Vector2 _initialPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _initialPosition = transform.position;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_isDragging)
            {
                // Refrescar la cámara en cada frame para manejar transiciones de escena
                if (_mainCamera == null)
                    _mainCamera = Camera.main;
                
                if (_mainCamera == null) return;
                
                Vector2 pointerPosition = MouseClicks.Instance.PointerPosition;
                Vector2 pointerWorldPosition = _mainCamera.ScreenToWorldPoint(pointerPosition);
                
                float clampedX = Mathf.Clamp(pointerWorldPosition.x, _initialPosition.x - clampRange, _initialPosition.x + clampRange);
                float clampedY = Mathf.Clamp(pointerWorldPosition.y, _initialPosition.y - clampRange, _initialPosition.y + clampRange);
                
                clampedX = Mathf.Lerp(_rigidbody.position.x, clampedX, dragSpeed * Time.deltaTime);
                clampedY = Mathf.Lerp(_rigidbody.position.y, clampedY, dragSpeed * Time.deltaTime);
                
                clampedX = useXAxis ? clampedX : _rigidbody.position.x;
                clampedY = useYAxis ? clampedY : _rigidbody.position.y;
                
                Vector2 clampedPosition = new Vector2(clampedX, clampedY);

                
                _rigidbody.MovePosition(clampedPosition);
            }
        }

        public void OnClick()
        {
            _isDragging = true;
        }

        public void OnClickUp()
        {
            _isDragging = false;
        }
    }
}
