using System.Collections;
using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        // Reference to global volume
        [SerializeField] private UnityEngine.Rendering.Volume globalVolume;
        [SerializeField] private UnityEngine.Rendering.Universal.Vignette vignetteEffect;

        private void Start()
        {
            if (!globalVolume)
            {
                Debug.LogError("Global Volume is not assigned in PostProcessingManager.");
                return;
            }

            if (!globalVolume.profile.TryGet(out vignetteEffect))
            {
                Debug.LogError("Vignette effect is not found in the global volume profile.");
            }
        }

        #region Vignette Effect

        public void SetVignetteIntensity(float intensity, float duration)
        {
            StartCoroutine(FadeVignette(intensity, duration));
        }

        private IEnumerator FadeVignette(float targetIntensity, float duration)
        {
            if (!vignetteEffect)
            {
                yield break;
            }

            float startIntensity = vignetteEffect.intensity.value;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                vignetteEffect.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
                yield return null;
            }

            vignetteEffect.intensity.value = targetIntensity;
        }

        #endregion
        
        
    }
}