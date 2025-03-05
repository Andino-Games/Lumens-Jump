using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Platforms;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Systems.Procedural
{
    public class LevelGenerator : MonoBehaviour
    { 
        [SerializeField] private Transform lastInitialPlatform;
        public Transform playerTransform;
        public PolygonCollider2D mapBoundsCollider;

        public float distanceBetweenPlatforms = 3f;

        private float lastPlatformY;

        [SerializeField]
        private List<Platform> platforms;
        public Transform platformHolder;
        public int minPoolSize = 10;
        public int maxPoolSize = 50;
        
        private ObjectPool<Platform> platformPool;

        private bool initPoolFunction;

        private void Start()
        {
            lastPlatformY = lastInitialPlatform.position.y;
            
            Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0f));
            Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));

            topLeft.y += 2.72f; 
            bottomRight.y += 2.72f; 
            
            Vector2[] points =
            {
                new Vector2(topLeft.x, topLeft.y),
                new Vector2(-topLeft.x, topLeft.y),
                new Vector2(bottomRight.x, bottomRight.y),
                new Vector2(-bottomRight.x, bottomRight.y)
            };

            mapBoundsCollider.points = points;
            
            PlayerPrefs.SetFloat("HighScore", 100);
            float variable = PlayerPrefs.GetFloat("HighScore", 0);
            
            Initialize();
        }

        private Platform CreatePlatform()
        {
            if (platforms.Count == 0)
            {
                Debug.LogError("Platforms list is empty! Make sure it's assigned in the Inspector.");
                return null;
            }
            
            int rnd = Random.Range(0, platforms.Count);

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
        
        public void Spawn()
        {
            platformPool.Get();
        }
        
        public void ResetLevel()
        {
            // 🗑️ Eliminar TODAS las plataformas existentes
            foreach (Transform platform in platformHolder)
            {
                Destroy(platform.gameObject);
            }

            // 📍 Reiniciar contador de altura
            lastPlatformY = 0;

            // 🏗️ Volver a generar el nivel como si estuviera empezando
            Initialize();
        }
    }
}

