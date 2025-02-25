using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Platforms
{
    public class BreakPlatform : MonoBehaviour
    {
        public float breakTime = 2f;
    
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
            
                yield return new WaitForSeconds(breakTime);
            
                onBreak.Invoke();
            }
        }

        

    }
}
