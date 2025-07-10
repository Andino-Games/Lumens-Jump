using UnityEngine;

namespace Systems.UI
{
    public class TriggerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator mainMenuAnimator;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Handle")) 
            {
                mainMenuAnimator.SetBool("Activate", true);
            }
        }
    }
}
