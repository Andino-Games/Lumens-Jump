using System.Collections;
using UnityEngine;

namespace Systems.PowerUps.Components
{
    public abstract class PowerUpComponent : MonoBehaviour
    {
        [HideInInspector] public GameObject player;
        public PowerUpComponentId powerUpComponentId;
        public bool isEnabled = true;
    
        [HideInInspector] public PowerUp powerUp;

        #region Unity Methods

        private void Awake()
        {
            player = gameObject;
        }

        private void Start()
        {
            SetUpComponents();
        }

        public void ExecutePowerUp()
        {
            if (isEnabled)
            {
                StartCoroutine(Execute());
            }
        }

        #endregion

        protected abstract void SetUpComponents();
        protected abstract IEnumerator Execute();
    }
}