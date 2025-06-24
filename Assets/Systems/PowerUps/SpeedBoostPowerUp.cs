using System.Collections;
using UnityEngine;

namespace Components
{
    public class SpeedBoostPowerUp : PowerUpComponent
    {
        public float multiplier = 2f;

        private Rigidbody2D _playerRigidbody;
        
        public override void SetUpComponents()
        {
            _playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        public override IEnumerator Execute()
        {
            if (!_playerRigidbody)
            {
                yield break;
            }

            // Apply speed boost
            _playerRigidbody.linearVelocity *= multiplier;
            Debug.Log($"Speed Boost Activated: Multiplier = {multiplier}");

            // Wait for a duration (e.g., 5 seconds)
            yield return new WaitForSeconds(5f);

            // Revert speed boost
            _playerRigidbody.linearVelocity /= multiplier;
            Debug.Log("Speed Boost Deactivated");
        }
    }
}