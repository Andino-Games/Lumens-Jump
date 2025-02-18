using Systems.Power_Ups.Base_Class;
using UnityEngine;

namespace Systems.Power_Ups
{
    public class Trampoline : PowerUpBase
    {
        private EPowerUpType powerUpType = EPowerUpType.Trampoline;

        protected override EPowerUpType PowerUpType => powerUpType;
        
        public override void Update()
        {
            Debug.Log($"{PowerUpType} is updating...");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActivatePowerUp();
            }
            
        }
        
        protected override void ActivatePowerUp()
        {
            Debug.Log($"This PowerUp {PowerUpType} is being used");
            // Implement trampoline behavior here (e.g., apply a bounce force).
        }
    }
}