using UnityEngine;

namespace Systems.Player
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
        }

    
    }
}
