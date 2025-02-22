using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Platforms
{
    public class BreakPlatform : MonoBehaviour
    {
        public bool hasContact;
    
        public UnityEvent onBreak;

        private void Update()
        {
            if (hasContact)
            {
                Break();
            }
        }

        private void Break()
        {
            onBreak.Invoke();
        }

        public void DebugBreak()
        {
            Debug.Log("Debug Break");
        }
    
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name);
            
                yield return new WaitForSeconds(1);
            
                hasContact = true;
            }
        }

        private IEnumerator OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                hasContact = false;
            }

            return null;
        }

    }
}
