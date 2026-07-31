using MoreMountains.Feedbacks;
using UnityEngine;

namespace Systems.Level.Feel
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
