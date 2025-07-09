using Systems.Level.Data;
using Systems.Manager;
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
            if (!_platformData.hasBeenUsed)
            {
                _platformData.hasBeenUsed = true;
                onPlatformUsed.Invoke();
            }
        }

        protected virtual void OnDestroyedPlatform()
        {
            onPlatformUsed.RemoveAllListeners();
        }

        protected void DestroyPlatform() 
        {
            OnDestroyedPlatform();
            _pool.Release(this);
        }
        
        #region Unity Events

        private Transform _playerTransform;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _playerTransform = GameInstances.Instance.player.transform;
        }

        private void Update()
        {

            if (_playerTransform.position.y > _transform.position.y)
            {
                OnUsedPlatform();
            }
            
            OnUpdatePlatform();
        }
        
        #endregion
        
        private bool IsOverPlatform(Transform other, string otherTag)
        {
            return other.CompareTag(otherTag) && other.transform.position.y >= transform.position.y;
        }
        
    }
}