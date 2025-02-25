using UnityEngine;

namespace Systems.Procedural
{
    public abstract class Generator : MonoBehaviour
    {
        protected virtual void Start()
        {
            Initialize();
        }
        
        protected abstract void Initialize();
        
        protected abstract void Spawn();
        
    }
}
