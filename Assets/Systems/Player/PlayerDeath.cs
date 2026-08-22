using System.Collections;
using Systems.Audio;
using Systems.Manager;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Systems.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private float reviveJumpForce;

        public Action onGameOver;
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
            if (CameraManager.HasInstance) CameraManager.Instance.SetCamera("Dead");
            AudioManager.Instance?.PlaySfx("Dead", 1);
            PostProcessingManager.Instance?.SetColorAdjustments(deathColor, 0.5f);
            PostProcessingManager.Instance?.SetVignetteIntensity(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 1f;
            onGameOver.Invoke();
        }

        public void ResetGame()
        {
            Debug.Log("Reviviendo al jugador de forma exitosa!");

            _isDead = false;

            _rigidbody2D.gravityScale = 1f;
            _rigidbody2D.linearVelocity = Vector2.zero;


            _rigidbody2D.AddForce(Vector2.up * reviveJumpForce, ForceMode2D.Impulse);
            
            anim.SetTrigger("Jump");

            _playerEffects.PlayMoveEffect();
        }
    }
}
