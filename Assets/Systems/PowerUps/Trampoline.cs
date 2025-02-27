using System.Collections;
using Systems.Player;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Trampoline : PowerUpBase
    {
        public float impulseForce;

        private GameObject player;
        
        [SerializeField] private Animator animator;

        public override void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        protected override IEnumerator HandlePowerUp()
        {
            Rigidbody2D rb = PlayerGameObject.GetComponent<Rigidbody2D>();
            PlayerJump playerJump = player.GetComponent<PlayerJump>();
            
            playerJump.isJumping = true;
            animator.SetTrigger("Activate");
            
            rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
            
            playerJump.playerCamera.Follow = player.transform;
            Debug.Log("Following Player");

            yield return new WaitForSeconds(1.2f);
            
            playerJump.playerCamera.Follow = null;
        }
    }
}