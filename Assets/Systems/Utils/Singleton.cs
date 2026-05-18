using UnityEngine;

namespace Systems.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindAnyObjectByType<T>();
                }

                return _instance;
            }

            private set => _instance = value;
        }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}