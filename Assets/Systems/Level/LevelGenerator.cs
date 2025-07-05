using System.Collections.Generic;
using Systems.Level.Data;
using UnityEngine;
using UnityEngine.Pool;
using Systems.Level.Platforms;

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
        [SerializeField] private float spawnInterval = 2f;

        private readonly List<ObjectPool<Platform>> _pools = new();
        private readonly List<Platform> _active = new();
        private float _nextSpawnY;
        private float _startReferenceY;
        
        private float _leftScreenX;
        private float _rightScreenX;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            if (!mainCamera)
            {
                Debug.LogError("No se encontró la cámara principal. Asegúrate de que haya una cámara con la etiqueta 'MainCamera'.");
                return;
            }
            
            if(!playerTransform)
            {
                Debug.LogError("No se ha asignado el transform del jugador. Por favor, asígnalo en el inspector.");
                return;
            }
            
            // Configuramos los límites de la pantalla
            _leftScreenX  = mainCamera.ScreenToWorldPoint(Vector3.zero).x;
            _rightScreenX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            
            _nextSpawnY = playerTransform.position.y + spawnInterval;
            
            // Para cada prefab creamos una pool y la precalentamos
            foreach (Platform prefab in platformPrefabs)
            {
                ObjectPool<Platform> pool = new ObjectPool<Platform>(
                    createFunc: () =>
                    {
                        return Instantiate(prefab, platformHolder);
                    },
                    actionOnGet: plat =>
                    {
                        plat.gameObject.SetActive(true);
                        _active.Add(plat);
                    },
                    actionOnRelease: plat =>
                    {
                        plat.gameObject.SetActive(false);
                        _active.Remove(plat);
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
            _startReferenceY = playerTransform.position.y;
            _nextSpawnY = 0;
            for (int i = 0; i < initialSpawnCount; i++)
            {
                _nextSpawnY += spawnInterval;
                Spawn();
            }
        }

        private void Update()
        {
            // Spawn según la posición del jugador
            if (playerTransform.position.y >= _nextSpawnY)
            {
                _nextSpawnY += spawnInterval;
                Spawn();
            }
        }

        private void Spawn()
        {
            // Elegir pool aleatoria
            int idx = Random.Range(0, _pools.Count);
            ObjectPool<Platform> pool = _pools[idx];
            Platform plat = pool.Get();

            // Calcular PlatformData
            float spawnY = _nextSpawnY;
            float spawnX = Random.Range(_leftScreenX, _rightScreenX);

            PlatformData data = new PlatformData
            {
                position    = new Vector3(spawnX, spawnY, 0f),
                hasBeenUsed = false,
                points      = 1
            };

            // Inicializar con datos
            plat.Initialize(pool, data);
        }

        public void ResetLevel()
        {
            // devolver todas las plataformas activas
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                _active[i].DestroyPlatform();
            }

            // recalcular el siguiente Y de spawn
            _nextSpawnY = playerTransform.position.y + spawnInterval;
        }
    }
}