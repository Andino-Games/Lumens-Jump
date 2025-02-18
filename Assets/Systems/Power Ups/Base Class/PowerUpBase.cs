using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Power_Ups.Base_Class
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract class PowerUpBase : MonoBehaviour
    {
        protected abstract EPowerUpType PowerUpType { get; }

        // Ensure that the PowerUp is registered correctly on Start.
        public virtual void Start()
        {
            Debug.Log($"This powerUp is of type: {PowerUpType}");
        }
        
        public abstract void Update();

        protected abstract void ActivatePowerUp();

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"The Player has entered the PowerUp Trigger: {PowerUpType}");
                ActivatePowerUp();
            }
        }
    }

    public enum EPowerUpType
    {
        Base,
        Trampoline,
        Rocket,
        Zipline,
    }
}