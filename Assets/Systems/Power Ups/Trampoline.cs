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
            
        }

        protected override void ActivatePowerUp()
        {
            Debug.Log($"This PowerUp {PowerUpType} is being used");
            Destroy(gameObject, 0.3f);
            // Implement trampoline behavior here (e.g., apply a bounce force).
        }
    }
}