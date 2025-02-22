using System.Collections.Generic;
using UnityEngine;

namespace Systems.Procedural
{
    public class LevelGenerator : Generator
    {
        public List<GameObject> platforms = new List<GameObject>();
        public Transform playerTransform;
        public BoxCollider2D mapBoundsCollider;

        public float distanceBetweenPlatforms = 3f;
        public int initialPlatformCount = 5;

        [SerializeField] private List<GameObject> instancedPlatforms = new List<GameObject>();
        private float lastPlatformY;

        private void Update()
        {
            if (playerTransform.position.y - lastPlatformY > distanceBetweenPlatforms)
            {
                Spawn();
            }
        }

        protected override void Initialize()
        {
            for (int i = 0; i < initialPlatformCount; i++)
            {
                Spawn();
            }
        }

        protected override void Spawn()
        {
            Bounds bounds = mapBoundsCollider.bounds;
            float posy = lastPlatformY + distanceBetweenPlatforms;
            float posx = Random.Range(bounds.min.x, bounds.max.x);
        
            Vector3 platformSpawnPosition = new Vector3(posx, posy, 0);
            GameObject newPlatform = Instantiate(platforms[Random.Range(0, platforms.Count)], platformSpawnPosition, Quaternion.identity);
            instancedPlatforms.Add(newPlatform);

            lastPlatformY = newPlatform.transform.position.y;
        }
    }
}
