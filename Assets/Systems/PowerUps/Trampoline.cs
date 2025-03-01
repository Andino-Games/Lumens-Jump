using System.Collections;
using Systems.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.PowerUps
{
    public class Trampoline : PowerUpBase
    {
        public float impulseForce;

        public bool powerActive;

        [SerializeField] private PlayerJump playerJump;
        
        [SerializeField] private Animator animator;
       

        public override void Start()
        {
            powerActive = false;

            if (playerJump == null)
            {
                playerJump = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJump>();
            }

            
        }

        public void RaycastPowerUp()
        {
            ActivatePowerUp();
        }

        protected override void ActivatePowerUp()
        {
            if (powerActive) return;
            else if (!powerActive)
            {
                base.ActivatePowerUp();
            }
            
        }

        protected override IEnumerator HandlePowerUp()
        {
            playerJump.Trampoline(impulseForce, powerActive);

            animator.SetTrigger("Activate");
            

            yield return new WaitForSeconds(1.5f);

            powerActive = false;

        }
    }
}