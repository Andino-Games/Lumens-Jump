using Systems.Level.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Systems.Level.Platforms
{
    public abstract class Platform : MonoBehaviour
    {
        public UnityEvent onPlatformUsed;
        
        private ObjectPool<Platform> _pool;
        private PlatformData _platformData;
        private GameObject _powerUp;
        
        public void Initialize(ObjectPool<Platform> pool, PlatformData platformData)
        {
            _pool = pool;
            _platformData = platformData;
            
            OnCreatedPlatform(_platformData);
        }

        protected virtual void OnUpdatePlatform() {}

        protected virtual void OnCreatedPlatform(PlatformData platformData)
        {
            _platformData = platformData;
            _platformData.hasBeenUsed = false;
        }

        protected virtual void OnUsedPlatform()
        {
            _platformData.hasBeenUsed = true;
            onPlatformUsed.Invoke();
        }

        protected virtual void OnDestroyedPlatform()
        {
            onPlatformUsed.RemoveAllListeners();
        }

        public virtual void SetPowerUp(Transform powerUp)
        {
            _powerUp = powerUp.gameObject;
            powerUp.SetParent(_transform);
            powerUp.localPosition = Vector3.zero + new Vector3(0, 0.5f, 0);
         }

        protected void DestroyPlatform() 
        {
            if (_powerUp)
            {
                Destroy(_powerUp);
            }
            OnDestroyedPlatform();
            _pool.Release(this);
        }
        
        #region Unity Events

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_platformData.hasBeenUsed)
            {
                OnUsedPlatform();
            }

            if (other.CompareTag("DestroyPlatform"))
            {
                DestroyPlatform();
            }
        }

        private void Update()
        {
            OnUpdatePlatform();
        }
        
        #endregion
    }
}