using System.Collections;
using Systems.Platforms.Feel;
using UnityEngine;

namespace Systems.Platforms
{
    public class BreakPlatform : Platform
    {
        public float breakTime = 2f;

        public bool isPlayerDownwards;

        public LayerMask playerLayer;

        public PlatformFeedbacks platformFeedbacks;

        [SerializeField] private GameObject platformObject;

        private void FixedUpdate()
        {
            isPlayerDownwards = Physics2D.Raycast(transform.position, Vector2.down, float.NegativeInfinity, playerLayer);

            if (isPlayerDownwards)
            {
                Debug.Log("Player is downwards");
                platformObject.SetActive(true);
            }
            else if (!isPlayerDownwards)
            {
               Debug.Log("Player is not downwards");
            }
        }

        public void Break(float time)
        {
            platformObject.SetActive(false);
        }
    
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !isPlayerDownwards)
            {
                Debug.Log(other.name);
            
                yield return new WaitForSeconds(breakTime);
            
                platformFeedbacks?.BreakablePlatform();
                
                Break(0.8f);
            }
        }

        

    }
}
