using MoreMountains.Feedbacks;
using UnityEngine;


namespace Systems.Platforms.Feel
{
    public class PlatformFeedbacks : MonoBehaviour
    {
        public MMF_Player breakablePlatformFeedback;
    
        public void BreakablePlatform()
        {
            breakablePlatformFeedback?.PlayFeedbacks();
        }
    }
}
