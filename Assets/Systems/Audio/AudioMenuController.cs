using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Systems.Audio
{
    public class AudioMenuController : MonoBehaviour
    {
        [SerializeField] private AudioMixer myMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Toggle musicButton;
        [SerializeField] private Toggle sfxButton;
        [SerializeField] private Toggle masterButton;
    
        private void Start()
        {
            if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
            {
                LoadVolume();
            }
            else 
            {
                SetMusicVolume();
                SetSfxVolume();
                SetMasterVolume();
            }
        }

        private void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        
            SetMusicVolume();
            SetSfxVolume();
            SetMasterVolume();
        }

        public void ToggleMusic()
        {
            AudioManager.Instance.ToggleMusic();
        }

        public void ToggleSfx()
        {
            AudioManager.Instance.ToggleSfx();
        }

        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
        
        public void SetSfxVolume()
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }

        public void SetMasterVolume()
        {
            float volume = masterSlider.value;
            myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("masterVolume", volume);
        }
    }
}