using System.Collections;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Zipline : PowerUpBase
    {
        public Transform startPoint;
        public Transform endPoint;
        public float zipLineSpeed = 0.1f;
        
        protected override IEnumerator HandlePowerUp()
        {
            Rigidbody2D rb = PlayerGameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.position = startPoint.position;

                float journeyLength = Vector2.Distance(startPoint.position, endPoint.position);
                float startTime = Time.time;

                while (Vector2.Distance(rb.position, endPoint.position) > 0.1f)
                {
                    float distCovered = (Time.time - startTime) * zipLineSpeed;
                    float fractionOfJourney = distCovered / journeyLength;
                    rb.position = Vector2.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
                    yield return null;
                }

                // Ensure the player reaches the end point.
                rb.position = endPoint.position;
                    
                Debug.Log($"{powerUpName} PowerUp completed!");
                
                rb.gravityScale = Mathf.Lerp(0f, 1f, 0.5f);
            }

            
            Destroy(gameObject); //optional destruction of object
        }
    }
}
