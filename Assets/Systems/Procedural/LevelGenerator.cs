using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Systems.Procedural
{
    public class LevelGenerator : MonoBehaviour
    {
        public Transform playerTransform;
        public BoxCollider2D mapBoundsCollider;

        public GameObject levelParent;

        public float distanceBetweenPlatforms = 3f;
        public int initialPlatformCount = 5;

        private float lastPlatformY;

        [SerializeField]
        private List<Platform> platforms;
        public Transform platformHolder;
        public int minPoolSize = 10;
        public int maxPoolSize = 50;

        private ObjectPool<Platform> platformPool;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (playerTransform.position.y - lastPlatformY > distanceBetweenPlatforms)
            {
                Spawn();
            }
        }

        private Platform CreatePlatform()
        {
            int rnd = Random.Range(0, 3);

            Platform platform = Instantiate(platforms[rnd], Vector3.zero, Quaternion.identity, platformHolder);

            platform.SetPool(platformPool);

            return platform;

        }

        private void OnTakePlatformFromPool(Platform platform)
        {

            Bounds bounds = mapBoundsCollider.bounds;
            float posy = lastPlatformY + distanceBetweenPlatforms;
            float posx = Random.Range(bounds.min.x, bounds.max.x);

            Vector3 platformSpawnPosition = new (posx, posy, 0);

            platform.transform.position = platformSpawnPosition;

            lastPlatformY = platform.transform.position.y;

            platform.gameObject.SetActive(true);
        }


        private void OnReturnPlatformFromPool(Platform platform)
        {
            platform.gameObject.SetActive(false);
            platform.transform.position = Vector3.zero;
        }

        private void OnDestroyPlatform(Platform platform)
        {
            Destroy(platform.gameObject);
        }



        protected void Initialize()
        {
            platformPool = new(CreatePlatform, OnTakePlatformFromPool, OnReturnPlatformFromPool, OnDestroyPlatform, true, minPoolSize, maxPoolSize);
        }

        protected void Spawn()
        {
            platformPool.Get();
        }
    }
}

