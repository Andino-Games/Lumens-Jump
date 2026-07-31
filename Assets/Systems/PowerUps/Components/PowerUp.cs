using System.Collections.Generic;
using UnityEngine;

namespace Systems.PowerUps.Components
{
    [AddComponentMenu("Andino Games/PowerUps/Object PowerUp")]
    public class PowerUp : MonoBehaviour
    {
        public List<PowerUpComponentId> powerUpComponentIds = new List<PowerUpComponentId>();
        private Collider2D _collider;
        private Animator _animator;
    
        public Animator Animator => _animator;
    
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animator = TryGetComponent(out Animator animator) ? animator : GetComponentInChildren<Animator>();

            if (!_collider)
            {
                throw new MissingComponentException("Collider2D not found on PowerUp GameObject. Please add a Collider2D component to the PowerUp GameObject.");
            }
        
            _collider.isTrigger = true;
        }
    }
}