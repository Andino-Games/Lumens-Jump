using UnityEngine;

namespace Systems.Power_Ups.Base_Class
{
    public abstract class PowerUpBase : MonoBehaviour
    {
        protected abstract EPowerUpType PowerUpType { get; }

        // Ensure that the PowerUp is registered correctly on Start.
        public virtual void Start()
        {
            Debug.Log($"This power-up is of type: {PowerUpType}");
        }
        
        public abstract void Update();

        protected abstract void ActivatePowerUp();
    }

    public enum EPowerUpType
    {
        Base,
        Trampoline,
        Rocket,
        Zipline,
    }
}