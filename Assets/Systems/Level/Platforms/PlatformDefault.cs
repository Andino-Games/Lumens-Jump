using Systems.Level.Data;
using UnityEngine;

namespace Systems.Level.Platforms
{
    public class PlatformDefault : Platform
    {
        private Vector3 _destinationPosition;
        
        #region Platform Methods

        protected override void OnCreatedPlatform(PlatformData platformData)
        {
            base.OnCreatedPlatform(platformData);

            _destinationPosition = platformData.position;
            
            int randomSign = Random.Range(0, 2) == 0 ? -1 : 1;
            Vector3 position = _destinationPosition + new Vector3(randomSign * 6, 0, 0);
            
            transform.position = position;
            
            LeanTween.move(gameObject, _destinationPosition, 1f)
                .setEase(LeanTweenType.easeInOutSine);
        }

        #endregion
    }
}
