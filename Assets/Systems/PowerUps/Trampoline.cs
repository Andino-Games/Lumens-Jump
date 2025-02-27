using System.Collections;
using Systems.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Trampoline : PowerUpBase
    {
        public float impulseForce;

        [SerializeField] private GameObject player;
        
        [SerializeField] private Animator animator;
        
        [SerializeField] private CinemachineCamera playerCamera;
        
        [SerializeField] private Collider2D playerCollider;

        public override void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            if (playerCamera == null)
            {
                playerCamera = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineCamera>();
            }

            if (playerCollider == null)
            {
                playerCollider = player.GetComponent<Collider2D>();
            }
        }

        protected override IEnumerator HandlePowerUp()
        {
            
            Rigidbody2D rb = PlayerGameObject.GetComponent<Rigidbody2D>();
            PlayerJump playerJump = player.GetComponent<PlayerJump>();
            
            playerJump.isJumping = true;
            animator.SetTrigger("Activate");
            
            
            playerCamera.Follow = playerJump.transform;
            
            rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
            

            yield return new WaitForSeconds(1.5f);
            
            
            playerJump.playerCamera.Follow = null;
        }
    }
}