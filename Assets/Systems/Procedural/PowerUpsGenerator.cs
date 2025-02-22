using System.Collections.Generic;
using UnityEngine;

namespace Systems.Procedural
{
    public class PowerUpsGenerator : Generator
    {
        public List<GameObject> powerUps = new List<GameObject>();
        
        public BoxCollider2D mapBoundsCollider;
        
        public float distanceBetweenPowerUps = Random.Range(4.5f, 8.5f);
        public int powerUpsCount = 4;
        
        [SerializeField] private List<GameObject> instancedPowerUps = new List<GameObject>();
        public float lastPowerUpPosition;
        
        
        protected override void Initialize()
        {
            for (int i = 0; i < powerUpsCount; i++)
            {
                Spawn();
            }
        }

        protected override void Spawn()
        {
            Bounds bounds = mapBoundsCollider.bounds;
            float posY = lastPowerUpPosition + distanceBetweenPowerUps;
            float posX = Random.Range(bounds.min.x, bounds.max.x);
            
            Vector3 powerUpSpawnPosition = new Vector3(posX, posY, 0);
            GameObject newPowerUp = Instantiate(powerUps[Random.Range(0, powerUps.Count)], powerUpSpawnPosition, Quaternion.identity);
            instancedPowerUps.Add(newPowerUp);
            
            lastPowerUpPosition = newPowerUp.transform.position.y;
        }
    }
}
