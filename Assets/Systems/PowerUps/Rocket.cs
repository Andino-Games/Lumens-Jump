using System.Collections;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Rocket : PowerUpBase
    {
        public float rocketSpeed;
        public float rocketDuration;
        
        protected override IEnumerator HandlePowerUp()
        {
            Rigidbody2D rb = PlayerGameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 0;
                Vector2 playerPosition = rb.position;
                playerPosition.y += rocketSpeed;
            }
            
            yield return new WaitForSeconds(rocketDuration);
            
            rb.gravityScale = 1;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
