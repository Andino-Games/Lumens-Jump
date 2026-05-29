using System.Collections;
using Systems.Audio;
using Systems.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        public UnityEvent onGameOver;
        public Color deathColor;
        private PlayerEffects _playerEffects;
        private Rigidbody2D _rigidbody2D;
        private bool _isDead;

        private void Awake()
        {
            _playerEffects = GetComponent<PlayerEffects>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DeathZone"))
            {
                if (_isDead)
                {
                    return;
                }
                
                _isDead = true;
                StartCoroutine(HandleDeath());
            }
        }

        private IEnumerator HandleDeath()
        {
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.linearVelocity = Vector2.zero;
            _playerEffects.PlayerDeathEffect();
            Time.timeScale = 0.25f;
            CameraManager.Instance.SetCamera("Dead");
            AudioManager.Instance.PlaySfx("Dead", 1);
            PostProcessingManager.Instance.SetColorAdjustments(deathColor, 0.5f);
            PostProcessingManager.Instance.SetVignetteIntensity(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 1f;
            onGameOver.Invoke();
        }
    }
}