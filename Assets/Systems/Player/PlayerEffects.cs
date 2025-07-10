using UnityEngine;
using MoreMountains.Feedbacks;

namespace Systems.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [Header("Feedbacks")] 
        public MMF_Player moveFeedback;
        public MMF_Player jumpFeedback;
        
        public void PlayMoveEffect()
        {
            moveFeedback?.PlayFeedbacks();
        } 
        public void PlayJumpEffect()
        {
            jumpFeedback?.PlayFeedbacks();
        }
    }
}