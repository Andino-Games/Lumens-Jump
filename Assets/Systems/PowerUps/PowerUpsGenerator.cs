using System;
using System.Collections.Generic;
using Systems.PowerUps.Components;
using UnityEngine;

namespace Systems.PowerUps
{
    [Serializable]
    public class PowerUpElement
    {
        public PowerUp powerUp;
        public int percentageChance;
    }
    
    public class PowerUpsGenerator : MonoBehaviour
    {
        [SerializeField] private List<PowerUpElement> powerUps;

        public Transform GeneratePowerUp()
        {
            if (powerUps.Count == 0)
            {
                Debug.LogWarning("No power-ups available to generate.");
                return null;
            }

            int totalChance = 0;
            foreach (var element in powerUps)
            {
                totalChance += element.percentageChance;
            }

            int randomValue = UnityEngine.Random.Range(0, totalChance);
            int cumulativeChance = 0;

            foreach (var element in powerUps)
            {
                cumulativeChance += element.percentageChance;
                if (randomValue < cumulativeChance)
                {
                    return Instantiate(element.powerUp.transform, transform.position, Quaternion.identity);
                }
            }
            
            return null;
        }
        
    }
}
