using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Systems.Platforms
{
    public abstract class Platform : MonoBehaviour
    {
        private ObjectPool<Platform> platformPool;
        
        private PlatformContact contact;

        public virtual void Start()
        {
            if(contact == null)
                contact = GetComponent<PlatformContact>();
            else
                Debug.LogWarning("Platform contact does not exist!");
        }

        public void ReleasePlatform()
        {
            platformPool.Release(this);
        }

        public void PointCounter()
        {
            contact.AddPoints();
        }

        public void SetPool(ObjectPool<Platform> pool)
        {
            platformPool = pool;
        }
    }
}
