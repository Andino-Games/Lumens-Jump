using System;
using System.Collections;
using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        [SerializeField] private UnityEngine.Rendering.Volume globalVolume;

        [Header("Valores por Defecto")]
        [SerializeField] private Color defaultColorFilter = Color.white;
        [SerializeField] private float defaultVignetteIntensity = 0.3f;
        [SerializeField] private float defaultChromaticAberration = 0f;

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

            // Restaurar valores limpios al iniciar la escena
            ResetToDefaults();
        }

        private void OnDestroy()
        {
            // Restaurar valores limpios al descargar la escena para no contaminar el perfil en disco
            ResetToDefaults();
        }

        private Coroutine _vignetteCoroutine;
        private Coroutine _chromaticCoroutine;
        private Coroutine _colorAdjustmentsCoroutine;

        public void ResetToDefaults()
        {
            if (_vignetteCoroutine != null)
            {
                StopCoroutine(_vignetteCoroutine);
                _vignetteCoroutine = null;
            }
            if (_chromaticCoroutine != null)
            {
                StopCoroutine(_chromaticCoroutine);
                _chromaticCoroutine = null;
            }
            if (_colorAdjustmentsCoroutine != null)
            {
                StopCoroutine(_colorAdjustmentsCoroutine);
                _colorAdjustmentsCoroutine = null;
            }

            if (_vignetteEffect != null)
                _vignetteEffect.intensity.value = defaultVignetteIntensity;
            if (_chromaticAberrationEffect != null)
                _chromaticAberrationEffect.intensity.value = defaultChromaticAberration;
            if (_colorAdjustmentsEffect != null)
                _colorAdjustmentsEffect.colorFilter.value = defaultColorFilter;
        }

        #region Vignette Effect

        public void SetVignetteIntensity(float intensity, float duration)
        {
            if (_vignetteCoroutine != null) StopCoroutine(_vignetteCoroutine);
            _vignetteCoroutine = StartCoroutine(FadeVignette(intensity, duration));
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
            _vignetteCoroutine = null;
        }

        #endregion

        #region Chromatic Aberration Effect

        public void SetChromaticAberrationIntensity(float intensity, float duration)
        {
            if (_chromaticCoroutine != null) StopCoroutine(_chromaticCoroutine);
            _chromaticCoroutine = StartCoroutine(FadeChromaticAberration(intensity, duration));
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
            _chromaticCoroutine = null;
        }

        #endregion
        
        #region Color Adjustments Effect
        
        public void SetColorAdjustments(Color color, float duration)
        {
            if (_colorAdjustmentsCoroutine != null) StopCoroutine(_colorAdjustmentsCoroutine);
            _colorAdjustmentsCoroutine = StartCoroutine(FadeColorAdjustments(color, duration));
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
            _colorAdjustmentsCoroutine = null;
        }
        
        #endregion
        
    }
}