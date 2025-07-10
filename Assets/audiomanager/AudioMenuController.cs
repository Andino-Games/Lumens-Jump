using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMenuController : MonoBehaviour
{
    //En estos campos poner los objetos de UI
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Toggle _musicButton;
    [SerializeField] private Toggle _sfxButton;
    [SerializeField] private Toggle _masterButton;


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else 
        {
            SetMusicVolume();
            SetSFXVolume();
            SetMasterVolume();
        }

    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        _masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }

    public void ToggleMusic() //llamar funcion en el OnClick del Boton
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX() //llamar funcion en el OnClick del Boton
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void SetMusicVolume() //llamar funcion en el OnValueChange del objeto
    {
        float volume = _musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
        
    public void SetSFXVolume() //llamar funcion en el OnValueChange del objeto
    {
        float volume = _sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetMasterVolume() //llamar funcion en el OnValueChange del objeto
    {
        float volume = _masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }


}
