using UnityEngine;
using MoreMountains.Feedbacks;

namespace Systems.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        private static readonly int Die = Animator.StringToHash("Die");

        [Header("Feedbacks")] 
        public MMF_Player moveFeedback;
        public MMF_Player jumpFeedback;
        public MMF_Player deathFeedback;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayMoveEffect()
        {
            moveFeedback?.PlayFeedbacks();
        } 
        public void PlayJumpEffect()
        {
            jumpFeedback?.PlayFeedbacks();
        }

        public void PlayerDeathEffect()
        {
            deathFeedback?.PlayFeedbacks();
            _animator.SetTrigger(Die);
        }
        
    }
}