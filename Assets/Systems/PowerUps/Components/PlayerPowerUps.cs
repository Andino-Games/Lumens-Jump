using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.PowerUps.Components
{
    [AddComponentMenu("Andino Games/PowerUps/Player PowerUps")]
    public class PlayerPowerUps : MonoBehaviour
    {
        private readonly Dictionary<PowerUpComponentId, PowerUpComponent> _powerUpComponents = new Dictionary<PowerUpComponentId, PowerUpComponent>();
        public List<PowerUpComponent> powerUps = new List<PowerUpComponent>();
    
        #if UNITY_EDITOR
        
        private void OnValidate()
        {
            LoadData();
        }

        #endif

        private void LoadData()
        {
            powerUps = GetComponents<PowerUpComponent>().ToList();
            _powerUpComponents.Clear();

            foreach (PowerUpComponent powerUp in powerUps)
            {
                if (!powerUp.powerUpComponentId)
                {
                    Debug.LogWarning($"PowerUp ({powerUp}) does not have a PowerUpComponentId assigned.", powerUp);
                    continue;
                }
                _powerUpComponents.TryAdd(powerUp.powerUpComponentId, powerUp);
            }
        }
        
        private void Start()
        {
        #if !UNITY_EDITOR
            LoadData();
        #endif
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PowerUp>(out PowerUp powerUp))
            {
                foreach (PowerUpComponentId componentId in powerUp.powerUpComponentIds)
                {
                    if (_powerUpComponents.TryGetValue(componentId, out PowerUpComponent powerUpComponent))
                    {
                        powerUpComponent.powerUp = powerUp;
                        powerUpComponent.ExecutePowerUp();
                    }
                    else
                    {
                        Debug.LogWarning($"PowerUpComponentId ({componentId}) not found in PlayerPowerUps.", this);
                    }
                }
            }
        
        }
    }
}