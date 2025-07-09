using System.Collections.Generic;
using Systems.Level.Data;
using UnityEngine;
using UnityEngine.Pool;
using Systems.Level.Platforms;
using Systems.Manager;
using Systems.PowerUps;

namespace Systems.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform platformHolder;

        [Header("Prefabs y Pool")]
        [SerializeField] private List<Platform> platformPrefabs;
        [SerializeField] private int initialPoolCount = 10;
        [SerializeField] private int maxPoolCount = 50;

        [Header("Generación")]
        [SerializeField] private int initialSpawnCount = 6;
        [SerializeField] private float spawnYInterval = 2f;
        [SerializeField] private float spawnXInterval = 0.5f;

        [Header("PowerUp Generator")]
        [SerializeField] private PowerUpsGenerator powerUpsGenerator;
        
        private readonly List<ObjectPool<Platform>> _pools = new();
        private float _nextSpawnY;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            if (!mainCamera)
            {
                Debug.LogError("No se encontró la cámara principal. Asegúrate de que haya una cámara con la etiqueta 'MainCamera'.", this);
                return;
            }
            
            if(!playerTransform)
            {
                Debug.LogError("No se ha asignado el transform del jugador. Por favor, asígnalo en el inspector.", this);
                return;
            }
            
            if (platformPrefabs == null || platformPrefabs.Count == 0)
            {
                Debug.LogError("No se han asignado prefabs de plataformas. Por favor, asígnalos en el inspector.", this);
                return;
            }
            
            InitializePools();
        }
        
        private void OnPlatformUsed()
        {
            GameManager.Instance.AddPoints(1);
            Spawn();
        }
        
        private void Spawn(ObjectPool<Platform> pool = null)
        {
            if (pool == null)
            {
                // Elegir pool aleatoria
                int idx = Random.Range(0, _pools.Count);
                pool = _pools[idx];
            }
            
            Platform plat = pool.Get();

            // Calcular PlatformData
            float spawnX = Random.Range(-spawnXInterval, spawnXInterval);
            
            PlatformData data = new PlatformData
            {
                position = new Vector3(spawnX, _nextSpawnY, 0f),
                hasBeenUsed = false,
            };
            
            _nextSpawnY += spawnYInterval;
            
            // Inicializar con datos
            plat.Initialize(pool, data);
            
            // Generar PowerUp si corresponde
            Transform powerUpTransform = powerUpsGenerator.GeneratePowerUp();
            if (powerUpTransform)
            {
                powerUpTransform.SetParent(plat.transform);
                powerUpTransform.localPosition = Vector3.zero + new Vector3(0f, 0.5f, 0f);
            }
        }

        private void InitializePools()
        {
            // Para cada prefab creamos una pool y la precalentamos
            foreach (Platform prefab in platformPrefabs)
            {
                ObjectPool<Platform> pool = new ObjectPool<Platform>(
                    createFunc: () =>
                    {
                        Platform platform = Instantiate(prefab, platformHolder);
                        platform.gameObject.SetActive(false);
                        return platform;
                    },
                    actionOnGet: platform =>
                    {
                        platform.gameObject.SetActive(true);
                        platform.onPlatformUsed.AddListener(OnPlatformUsed);
                    },
                    actionOnRelease: platform =>
                    {
                        platform.gameObject.SetActive(false);
                        platform.onPlatformUsed.RemoveListener(OnPlatformUsed);
                    },
                    actionOnDestroy: plat =>
                    {
                        Destroy(plat.gameObject);
                    },
                    collectionCheck: true,
                    defaultCapacity: initialPoolCount,
                    maxSize:         maxPoolCount
                );

                // precalentamos al menos initialPoolCount instancias
                for (int i = 0; i < initialPoolCount; i++)
                {
                    Platform temp = pool.Get();
                    pool.Release(temp);
                }

                _pools.Add(pool);
            }
            
            // Spawnear plataformas iniciales
            ObjectPool<Platform> defaultPool = _pools[0];
            _nextSpawnY = playerTransform.position.y + spawnYInterval - 0.5f;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                Spawn(defaultPool);
            }
        }
    }
}