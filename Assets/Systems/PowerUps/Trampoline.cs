using System.Collections;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Trampoline : PowerUpBase
    {
        public float impulseForce;
        
        protected override IEnumerator HandlePowerUp()
        {
            Rigidbody2D rb = PlayerGameObject.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
                Debug.Log($"{powerUpName} is used");
            }
            yield return null;
        }
    }
}