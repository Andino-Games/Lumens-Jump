using System.Collections;
using Systems.Platforms.Feel;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Platforms
{
    public class BreakPlatform : MonoBehaviour
    {
        public float breakTime = 2f;
    
        public PlatformFeedbacks platformFeedbacks;

        

        public void Break(float time)
        {
            Destroy(gameObject, time);
        }
    
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name);
            
                yield return new WaitForSeconds(breakTime);
            
                platformFeedbacks?.BreakablePlatform();
                
                Break(0.8f);
            }
        }

        

    }
}
