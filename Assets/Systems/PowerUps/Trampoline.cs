using System.Collections;
using Systems.Player;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Trampoline : PowerUpBase
    {
        public float impulseForce;

        private GameObject player;

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

            yield return null;
        }
    }
}