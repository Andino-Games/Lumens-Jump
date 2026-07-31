using System.Collections;
using Systems.Audio;
using Systems.PowerUps.Components;
using UnityEngine;

namespace Systems.PowerUps.Instances
{
    public class TrampolinePowerUp : PowerUpComponent
    {
        private static readonly int Activate = Animator.StringToHash("Activate");
        
        [SerializeField] private float jumpForce = 10f;
        
        private Rigidbody2D _playerRigidbody;
        private bool _isActive;

        protected override void SetUpComponents()
        {
            _playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        protected override IEnumerator Execute()
        {
            if (_isActive)
            {
                yield break;
            }
            
            powerUp.Animator.SetTrigger(Activate);
            
            _isActive = true;
            _playerRigidbody.linearVelocity = Vector2.zero;
            _playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySfx("Rocket_Start", 1);
            yield return new WaitForSeconds(1f);
            _isActive = false;
        }
    }
}