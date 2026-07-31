using UnityEngine;
using UnityEngine.Events;

namespace Systems.UI.Cordel
{
    public class CordelTriggerActivator : MonoBehaviour
    {
        public UnityEvent onCordelEnter;
        private bool _isCordelActive;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCordelActive)
            {
                return;
            }
           
            onCordelEnter.Invoke();
            _isCordelActive = true;
        }
    }
}