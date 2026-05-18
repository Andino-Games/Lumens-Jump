using System;
using System.Collections;
using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        [SerializeField] private UnityEngine.Rendering.Volume globalVolume;
        private UnityEngine.Rendering.Universal.Vignette _vignetteEffect;
        private UnityEngine.Rendering.Universal.ChromaticAberration _chromaticAberrationEffect;
        private UnityEngine.Rendering.Universal.ColorAdjustments _colorAdjustmentsEffect;

        private void Start()
        {
            if (!globalVolume)
            {
                Debug.LogError("Global Volume is not assigned in PostProcessingManager.");
                return;
            }

            if (!globalVolume.profile.TryGet(out _vignetteEffect))
            {
                Debug.LogError("Vignette effect is not found in the global volume profile.");
            }
            
            if (!globalVolume.profile.TryGet(out _chromaticAberrationEffect))
            {
                Debug.LogError("Chromatic Aberration effect is not found in the global volume profile.");
            }
            
            if (!globalVolume.profile.TryGet(out _colorAdjustmentsEffect))
            {
                Debug.LogError("Color Curves effect is not found in the global volume profile.");
            }
        }

        #region Vignette Effect

        public void SetVignetteIntensity(float intensity, float duration)
        {
            StartCoroutine(FadeVignette(intensity, duration));
        }

        private IEnumerator FadeVignette(float targetIntensity, float duration)
        {
            if (!_vignetteEffect)
            {
                yield break;
            }

            float startIntensity = _vignetteEffect.intensity.value;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                _vignetteEffect.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
                yield return null;
            }

            _vignetteEffect.intensity.value = targetIntensity;
        }

        #endregion

        #region Chromatic Aberration Effect

        public void SetChromaticAberrationIntensity(float intensity, float duration)
        {
            StartCoroutine(FadeChromaticAberration(intensity, duration));
        }
        
        private IEnumerator FadeChromaticAberration(float targetIntensity, float duration)
        {
            if (!_chromaticAberrationEffect)
            {
                yield break;
            }

            float startIntensity = _chromaticAberrationEffect.intensity.value;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                _chromaticAberrationEffect.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
                yield return null;
            }

            _chromaticAberrationEffect.intensity.value = targetIntensity;
        }

        #endregion
        
        #region Color Adjustments Effect
        
        public void SetColorAdjustments(Color color, float duration)
        {
            StartCoroutine(FadeColorAdjustments(color, duration));
        }

        private IEnumerator FadeColorAdjustments(Color targetColor, float duration)
        {
            if (!_colorAdjustmentsEffect)
            {
                yield break;
            }

            Color startColor = _colorAdjustmentsEffect.colorFilter.value;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                _colorAdjustmentsEffect.colorFilter.value = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            _colorAdjustmentsEffect.colorFilter.value = targetColor;
        }
        
        #endregion
        
    }
}