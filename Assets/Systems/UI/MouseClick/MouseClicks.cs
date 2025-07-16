using Systems.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.UI.MouseClick
{
    public class MouseClicks : Singleton<MouseClicks>
    {
        [SerializeField] private LayerMask clickableLayerMask;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction _mouseClickAction;
        private InputAction _mouseClickPositionAction;
        private Camera _camera;
        
        public Vector2 PointerPosition => _mouseClickPositionAction.ReadValue<Vector2>();
        
        private GameObject _mouseClickObject;

        protected override void Awake()
        {
            base.Awake();
            LoadActions();
        }

        private void LoadActions()
        {
            _mouseClickAction = inputActions.FindAction("UI/Click", true);
            _mouseClickPositionAction = inputActions.FindAction("UI/ClickPosition", true);
        }

        private void OnEnable()
        {
            _camera = Camera.main;
            
            LoadActions();
            
            _mouseClickAction.performed += OnClickDown;
            _mouseClickAction.canceled += OnClickUp;
        }
        
        private void OnDisable()
        {
            LoadActions();
            
            _mouseClickAction.performed -= OnClickDown;
            _mouseClickAction.canceled -= OnClickUp;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnClickDown(InputAction.CallbackContext obj)
        {
            if (_mouseClickObject)
            {
                IClickUp clickable = _mouseClickObject.GetComponent<IClickUp>();
                clickable?.OnClickUp();
                _mouseClickObject = null;
            }

            Vector2 mousePosition = _mouseClickPositionAction.ReadValue<Vector2>();
            Vector2 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, clickableLayerMask);
            if (hit.collider)
            {
                _mouseClickObject = hit.collider.gameObject;
                IClickDown clickable = _mouseClickObject.GetComponent<IClickDown>();
                clickable?.OnClick();
            }
        }
        
        private void OnClickUp(InputAction.CallbackContext obj)
        {
            if (_mouseClickObject)
            {
                IClickUp clickable = _mouseClickObject.GetComponent<IClickUp>();
                clickable?.OnClickUp();
                _mouseClickObject = null;
            }
        }
    }
}