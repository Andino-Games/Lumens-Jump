using UnityEngine;
using UnityEngine.Pool;

public abstract class Platform : MonoBehaviour
{
    private ObjectPool<Platform> platformPool;
    
    public void ReleasePlatform()
    {
        platformPool.Release(this);
    }

    public void SetPool(ObjectPool<Platform> pool)
    {
         platformPool = pool;
    }
}
