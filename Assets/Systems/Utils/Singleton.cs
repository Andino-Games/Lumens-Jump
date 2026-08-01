using UnityEngine;

namespace Systems.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static bool HasInstance
        {
            get
            {
                if (_instance) return true;
                _instance = FindAnyObjectByType<T>();
                return _instance != null;
            }
        }

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindAnyObjectByType<T>();
                }

                if (!_instance)
                {
                    var scriptName = typeof(T).Name;
                    var prefabLocation = $"Singletons/{scriptName}";
                    var prefab = Resources.Load<GameObject>(prefabLocation);
                    if (prefab != null)
                    {
                        _instance = Instantiate(prefab).GetComponent<T>();
                    }
                    else
                    {
                        Debug.LogError($"Singleton<{scriptName}>: No instance found in scene and no prefab at 'Resources/{prefabLocation}'.");
                    }
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