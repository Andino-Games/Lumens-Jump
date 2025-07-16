using System.Collections;
using Systems.Audio;
using Systems.Level.Data;
using UnityEngine;

namespace Systems.Level.Platforms
{
    public class PlatformBreak : Platform
    {
        [Header("Break Platform Settings")]
        [SerializeField] private float breakTime = 2f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private GameObject platformObject;

        [Header("Effects")]
        [SerializeField] private ParticleSystem landedEffect;
        [SerializeField] private ParticleSystem breakEffect;

        private Vector3 _destinationPosition;
        private bool _isUsed;

        #region Platform Methods

        protected override void OnCreatedPlatform(PlatformData platformData)
        {
            base.OnCreatedPlatform(platformData);
            _isUsed = false;
            
            landedEffect.Stop();
            breakEffect.Stop();

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
            if (_isUsed)
            {
                return;
            }
            StartCoroutine(Break());
        }

        #endregion

        private IEnumerator Break()
        {
            _isUsed = true;
            landedEffect.Play();
            yield return new WaitForSeconds(breakTime);
            breakEffect.Play();
            AudioManager.Instance.PlaySfx("Platform", 1);
            yield return new WaitForSeconds(breakTime);
            _isUsed = false;
            DestroyPlatform();
        }
    }
}