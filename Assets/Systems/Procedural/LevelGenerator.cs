using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        //Nuevo ajuste del espaciado de plataformas
        [Header("Platform spacing")]
        [SerializeField] private float minVerticalDistance = 2f;
        [SerializeField] private float maxVerticalDistance = 3.5f;
        [SerializeField] private float maxHorizontalOffset = 3f;
        //public float distanceBetweenPlatforms = 3f;

        private float lastPlatformY;
        private float lastPlatformX;//variable para controlar la posición X de la última plataforma generada

        [SerializeField]
        private List<Platform> platforms;
        public Transform platformHolder;
        public int minPoolSize = 10;
        public int maxPoolSize = 50;
        private float generationThreshold;

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

        //Integracion de metodo Update para la generación de plataformas
        private void Update()
        {
            if (playerTransform.position.y > generationThreshold)
            {
                Spawn();
                generationThreshold = lastPlatformY;
            }
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

            // Asegurarse de que la plataforma tenga un collider para calcular su ancho
            var platformCollider = platform.GetComponent<Collider2D>();
            if (platformCollider == null) 
            {
                Debug.LogError("La plataforma no tiene un collider para calcular su ancho");
                return;
            }

            float platformWidth = platformCollider.bounds.size.x / 2; // Ancho de la plataforma dividido por 2 para obtener el radio

            float spawnableBounsMinX = bounds.min.x + platformWidth; // posición mínima X de la nueva plataforma, asegurando que no se salga del mapa
            float spawnableBounsMaxX = bounds.max.x - platformWidth; // posición máxima X de la nueva plataforma, asegurando que no se salga del mapa

            float randomDistance = Random.Range(minVerticalDistance, maxVerticalDistance);// distancia aleatoria entre plataformas
            float posy = lastPlatformY + randomDistance;// posición Y de la nueva plataforma

            float minPosX = Mathf.Max(spawnableBounsMinX, lastPlatformX - maxHorizontalOffset);// posición mínima X de la nueva plataforma, asegurando que no se salga del mapa
            float maxPosX = Mathf.Min(spawnableBounsMaxX, lastPlatformX + maxHorizontalOffset);// posición máxima X de la nueva plataforma, asegurando que no se salga del mapa
            float posx = Random.Range(minPosX, maxPosX);

            Vector3 platformSpawnPosition = new (posx, posy, 0);

            platform.transform.position = platformSpawnPosition;

            lastPlatformY = platform.transform.position.y;
            lastPlatformX = platform.transform.position.x;

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
            
            generationThreshold = lastPlatformY;//se añadió para que la generación de plataformas comience desde la última plataforma generada

            for (int i = 0; i < minPoolSize; i++) //ciclo para llenar el pool con plataformas iniciales
            {
                Spawn();
            }
        }
        
        public void Spawn()
        {
            platformPool.Get();
        }
        
        public void ResetLevel()
        {
            
            foreach (Transform platform in platformHolder)
            {
                Destroy(platform.gameObject);
            }

            
            lastPlatformY = 0;

            
            Initialize();
        }
    }
}

