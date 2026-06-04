using Systems.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

namespace Systems.UI.MouseClick
{
    public class MouseClicks : Singleton<MouseClicks>
    {
        [SerializeField] private LayerMask clickableLayerMask;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction _mouseClickAction;
        private InputAction _mouseClickPositionAction;
        private Camera _camera;

        public Vector2 PointerPosition => _mouseClickPositionAction != null ? _mouseClickPositionAction.ReadValue<Vector2>() : Vector2.zero;
        
        private GameObject _mouseClickObject;

        protected override void Awake()
        {
            base.Awake();
            // Solo la instancia real del Singleton debe inicializarse.
            // Los duplicados son destruidos por base.Awake() y no necesitan LoadActions.
            if (Instance == this) LoadActions();
        }

        private void LoadActions()
        {
            _mouseClickAction = inputActions.FindAction("UI/Click", true);
            _mouseClickPositionAction = inputActions.FindAction("UI/ClickPosition", true);
        }

        private void OnEnable()
        {
            // Solo la instancia Singleton activa debe suscribirse a eventos
            if (Instance != this) return;
            
            LoadActions();
            
            _mouseClickAction.performed += OnClickDown;
            _mouseClickAction.canceled += OnClickUp;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnableInputActions();
        }
        
        private void OnDisable()
        {
            if (Instance != this) return;
            
            _mouseClickAction.performed -= OnClickDown;
            _mouseClickAction.canceled -= OnClickUp;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        // Se ejecuta cada vez que se carga una nueva escena
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // SOLUCIÓN: Reactivar las acciones de input para asegurar que
            // el sistema escuche los clics en la nueva escena.
            EnableInputActions();
        }

        private void EnableInputActions()
        {
            _mouseClickAction.Enable();
            _mouseClickPositionAction.Enable();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnClickDown(InputAction.CallbackContext obj)
        {
            if (_mouseClickObject)
            {
                if (_mouseClickObject.TryGetComponent(out IClickUp clickable))
                {
                    clickable.OnClickUp();
                }
                _mouseClickObject = null;
            }

            _camera = Camera.main;

            if (_camera == null)
            {
                Debug.LogWarning("MouseClicks: No active Main Camera found to process click.");
                return;
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