using UnityEngine;

namespace Systems.Player
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        
        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
    }
}
