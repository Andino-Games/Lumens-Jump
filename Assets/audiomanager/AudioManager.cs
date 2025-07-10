using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, ambSounds, uiSounds;
    public AudioSource musicSource, sfxSource, ambSource, uiSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Music");
        PlayAMB("Amb");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.loop = s.loop;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, float pitch)
    {
        Sound s2 = Array.Find(sfxSounds, x => x.name == name);

        if (s2 == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(s2.clip, s2.volume);
            //sfxSource.pitch = 1f;
        }
    }

    public void PlayUI(string name)
    {
        Sound s3 = Array.Find(uiSounds, x => x.name == name);

        if (s3 == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            uiSource.PlayOneShot(s3.clip, s3.volume);
        }
    }

    public void PlayAMB(string name)
    {
        Sound s4 = Array.Find(ambSounds, x => x.name == name);

        if (s4 == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            ambSource.clip = s4.clip;
            ambSource.volume = s4.volume;
            ambSource.loop = s4.loop;
            ambSource.Play();
        }
    }

    //UI Sound Controls
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
   











    //public void MusicVolume(float volume)
    //{
    //musicSource.volume = volume;
    //}
    // public void SFXVolume(float volume)
    // {
    // sfxSource.volume = volume;
    //}

    //To call the PlaySfx
    //AudioManager.Instance.PlaySFX("Nombre del SFX");

    //To stop music
    //AudioManager.Instance.musicSource.stop();

}
