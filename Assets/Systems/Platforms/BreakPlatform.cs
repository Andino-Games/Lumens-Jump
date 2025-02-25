using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Platforms
{
    public class BreakPlatform : MonoBehaviour
    {
        
    
        public UnityEvent onBreak;

        

        public void Break()
        {
            Destroy(gameObject);
        }
    
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name);
            
                yield return new WaitForSeconds(1);
            
                onBreak.Invoke();
            }
        }

        

    }
}
