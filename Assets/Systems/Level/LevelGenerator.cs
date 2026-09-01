using System.Collections.Generic;
using Systems.Level.Data;
using UnityEngine;
using UnityEngine.Pool;
using Systems.Level.Platforms;
using Systems.Manager;
using Systems.PowerUps;
using Systems.PowerUps.Instances;

namespace Systems.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform platformHolder;
        [SerializeField] private TutorialController tutorialController;

        [Header("Prefabs and Pool")]
        [SerializeField] private List<Platform> platformPrefabs;
        [SerializeField] private int initialPoolCount = 10;
        [SerializeField] private int maxPoolCount = 50;

        [Header("Generation Settings")]
        [SerializeField] private int initialSpawnCount = 6;
        [SerializeField] private float spawnYInterval = 2f;
        [SerializeField] private float spawnXInterval = 0.5f;

        [Header("PowerUp Generator")]
        [Range(0f,1f)][SerializeField] private float powerUpSpawnChance = 0.5f;
        [SerializeField] private PowerUpsGenerator powerUpsGenerator;
        
        private readonly List<ObjectPool<Platform>> _pools = new();
        private float _nextSpawnY;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            if (!mainCamera)
            {
                Debug.LogError("Main camera is not assigned in LevelGenerator.", this);
                return;
            }
            
            if(!playerTransform)
            {
                Debug.LogError("Player transform is not assigned in LevelGenerator.", this);
                return;
            }
            
            if (platformPrefabs == null || platformPrefabs.Count == 0)
            {
                Debug.LogError("Platform prefabs are not assigned in LevelGenerator.", this);
                return;
            }
            
            InitializePools();
        }
        
        private void OnPlatformUsed()
        {
            GameManager.Instance.AddPoints();
        }

        private const int MaxSpawnsPerFrame = 3;

        private void Update()
        {
            if (playerTransform == null) return;

            // Maintain a buffer of platforms ahead of the player's Y position.
            // (initialSpawnCount + 2) * spawnYInterval ensures there are plenty of platforms ahead of the player.
            float lookaheadDistance = (initialSpawnCount + 2) * spawnYInterval;
            
            // Limitar spawns por frame para evitar spikes de rendimiento
            for (int i = 0; i < MaxSpawnsPerFrame && _nextSpawnY < playerTransform.position.y + lookaheadDistance; i++)
            {
                if (tutorialController.IsActive == true)
                {
                    Spawn(_pools[0], false);
                }
                else
                {
                    Spawn();
                }
            }
        }
        
        private void Spawn(ObjectPool<Platform> pool = null, bool canSpawnPowerUps = true)
        {
            if (pool == null)
            {
                int idx = Random.Range(0, _pools.Count);
                pool = _pools[idx];
            }
            
            Platform plat = pool.Get();

            float spawnX = Random.Range(-spawnXInterval, spawnXInterval);
            
            PlatformData data = new PlatformData
            {
                position = new Vector3(spawnX, _nextSpawnY, 0f),
                hasBeenUsed = false,
            };
            
            _nextSpawnY += spawnYInterval;
            
            plat.Initialize(pool, data);

            if (canSpawnPowerUps == false)
            {
                return;
            }

            bool isPowerUpSpawned = Random.value < powerUpSpawnChance;
            
            if (isPowerUpSpawned)
            {
                Transform powerUpTransform = powerUpsGenerator.GeneratePowerUp();
                plat.SetPowerUp(powerUpTransform);
            }
        }

        private void InitializePools()
        {
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

                for (int i = 0; i < initialPoolCount; i++)
                {
                    Platform temp = pool.Get();
                    pool.Release(temp);
                }

                _pools.Add(pool);
            }
            
            ObjectPool<Platform> defaultPool = _pools[0];
            _nextSpawnY = playerTransform.position.y + spawnYInterval - 0.5f;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                Spawn(defaultPool, false);
            }
        }
    }
}