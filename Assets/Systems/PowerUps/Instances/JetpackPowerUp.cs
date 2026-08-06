using System.Collections;
using System.Collections.Generic;
using Systems.Audio;
using Systems.Manager;
using Systems.PowerUps.Components;
using UnityEngine;

namespace Systems.PowerUps.Instances
{
    public class JetpackPowerUp : PowerUpComponent
    {
        private static readonly int Activate = Animator.StringToHash("Activate");
        
        [SerializeField] private float transitionTime = 0.5f;
        [SerializeField] private float duration = 5f;
        [SerializeField] private float thrust = 10f;
        [SerializeField] private List<ParticleSystem> jetpackParticles;

        private Rigidbody2D _playerRigidbody;
        private bool _isActive;

        public bool IsActive => _isActive;

        protected override void SetUpComponents()
        {
            _playerRigidbody = player.GetComponent<Rigidbody2D>();

            foreach (ParticleSystem jetpackParticle in jetpackParticles)
            {
                jetpackParticle.Stop();
            }
        }

        protected override IEnumerator Execute()
        {
            if (_isActive)
            {
                yield break;
            }
            
            powerUp.Animator.SetTrigger(Activate);
            
            _isActive = true;
            AudioManager.Instance.PlaySfx("Rocket_Start", 1);
            Time.timeScale = 0.25f;
            
            if (CameraManager.HasInstance) CameraManager.Instance.SetCamera("Jetpack");
            PostProcessingManager.Instance?.SetVignetteIntensity(0.6f, transitionTime);
            PostProcessingManager.Instance?.SetChromaticAberrationIntensity(0.6f, transitionTime);
            
            yield return new WaitForSeconds(transitionTime);
            Time.timeScale = 1f;
            
            PostProcessingManager.Instance?.SetVignetteIntensity(0.45f, transitionTime * 2);
            
            foreach (ParticleSystem jetpackParticle in jetpackParticles)
            {
                jetpackParticle.Play();
            }
            
            _playerRigidbody.linearVelocity = Vector2.zero;
            _playerRigidbody.gravityScale = 0f;
            _playerRigidbody.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);
            
            PostProcessingManager.Instance?.SetVignetteIntensity(0.3f, transitionTime * 2);
            PostProcessingManager.Instance?.SetChromaticAberrationIntensity(0f, transitionTime * 2);

            foreach (ParticleSystem jetpackParticle in jetpackParticles)
            {
                jetpackParticle.Stop();
                AudioManager.Instance.PlaySfx("Rocket_End", 1);
            }
            
            _playerRigidbody.gravityScale = 1f;
            _playerRigidbody.linearVelocity = Vector2.zero;
            
            Time.timeScale = 0.25f;
            if (CameraManager.HasInstance) CameraManager.Instance.SetCamera("Default");

            yield return new WaitForSeconds(transitionTime);

            Time.timeScale = 1f;
            _isActive = false;
        }

    }
}