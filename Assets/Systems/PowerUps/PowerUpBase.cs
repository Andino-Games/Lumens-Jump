using System;
using System.Collections;
using UnityEngine;

namespace Systems.PowerUps
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract class PowerUpBase : MonoBehaviour
    {
        public string powerUpName;
        protected GameObject PlayerGameObject;
        protected SpriteRenderer SpriteRenderer;
        private Coroutine _powerUpRoutine;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Ensure that the PowerUp is registered correctly on Start.
        public virtual void Start()
        {
            Debug.Log($"This powerUp is of type: {powerUpName}");
        }

        protected virtual void ActivatePowerUp()
        {
            Debug.Log($"This PowerUp {powerUpName} is being used");
            SpriteRenderer.enabled = false;
            _powerUpRoutine = StartCoroutine(HandlePowerUp());
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"The Player has entered the PowerUp Trigger: {powerUpName}");
                PlayerGameObject = other.gameObject;
                ActivatePowerUp();
            }
        }

        protected abstract IEnumerator HandlePowerUp();

        // Stop Coroutine if the PowerUp is destroyed.
        private void OnDestroy()
        {
            if (_powerUpRoutine != null)
            {
                StopCoroutine(_powerUpRoutine);
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