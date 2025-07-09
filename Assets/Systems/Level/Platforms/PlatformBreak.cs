using System.Collections;
using Systems.Level.Data;
using Systems.Level.Feel;
using UnityEngine;

namespace Systems.Level.Platforms
{
    public class PlatformBreak : Platform
    {
        [Header("Break Platform Settings")]
        [SerializeField] private float breakTime = 2f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private GameObject platformObject;

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

        protected override void OnUsedPlatform()
        {
            base.OnUsedPlatform();
            
            StartCoroutine(Break());
        }

        #endregion

        private IEnumerator Break()
        {
            yield return new WaitForSeconds(breakTime);

            DestroyPlatform();
        }
    }
}
