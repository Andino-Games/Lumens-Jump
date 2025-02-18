using Systems.Power_Ups.Base_Class;
using UnityEngine;

namespace Systems.Power_Ups
{
    public class Rocket : PowerUpBase
    {
        private EPowerUpType powerUpType = EPowerUpType.Rocket;

        protected override EPowerUpType PowerUpType => powerUpType;

        public override void Update()
        {
            Debug.Log($"{PowerUpType} is updating...");
            
        }
        
        protected override void ActivatePowerUp()
        {
            Debug.Log($"This PowerUp {PowerUpType} is being used");
            // Implement trampoline behavior here (e.g., apply a bounce force).
        }
    }
}
