using System.Collections;
using Systems.Audio;
using UnityEngine;

namespace Systems.PowerUps
{
    public class TrampolinePowerUp : PowerUpComponent
    {
        public float jumpForce = 10f;
        
        private Rigidbody2D _playerRigidbody;
        private bool _isActive;
        
        public override void SetUpComponents()
        {
            _playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        public override IEnumerator Execute()
        {
            if (_isActive)
            {
                yield break;
            }
            
            _isActive = true;
            _playerRigidbody.linearVelocity = Vector2.zero;
            _playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySfx("Rocket_Start", 1);
            yield return new WaitForSeconds(1f);
            _isActive = false;
        }
    }
}