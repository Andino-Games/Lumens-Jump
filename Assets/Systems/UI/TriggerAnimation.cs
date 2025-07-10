using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _MainMenuAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Handle")) 
        {
            _MainMenuAnimator.SetBool("Activate", true);
        }
    }
}
