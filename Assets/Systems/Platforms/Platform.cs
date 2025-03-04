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
            contact = GetComponent<PlatformContact>();

            if (contact == null)
            {
                Debug.LogWarning("PlatformContact no encontrado en la plataforma.");
            }
        }

        public void ReleasePlatform()
        {
            platformPool.Release(this);
        }

        public void PointCounter()
        {
            if (contact != null)
            {
                contact.gameObject.SetActive(true); // Asegurar que esté activo al usarse
                contact.SendMessage("AddPoints", SendMessageOptions.DontRequireReceiver);
            }
        }

        public void SetPool(ObjectPool<Platform> pool)
        {
            platformPool = pool;
        }
    }
}
