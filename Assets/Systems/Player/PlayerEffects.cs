using UnityEngine;
using MoreMountains.Feedbacks; // Importante para usar Feel

namespace Systems.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [Header("Feedbacks")] 
        public MMF_Player moveFeedback; // Feedback para movimiento
        public MMF_Player jumpFeedback; // Feedback para salto
        public MMF_Player collisionPlatformFeedback; // Feedback para colisión con plataforma

        /// <summary>
        /// Activa el feedback de movimiento
        /// </summary>
        public void PlayMoveEffect()
        {
            moveFeedback?.PlayFeedbacks();
        }

        /// <summary>
        /// Activa el feedback de salto
        /// </summary>
        public void PlayJumpEffect()
        {
            jumpFeedback?.PlayFeedbacks();
            collisionPlatformFeedback?.StopFeedbacks(); 
        }
    }
}