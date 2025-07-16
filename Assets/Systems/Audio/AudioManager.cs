using System;
using Systems.Utils;
using UnityEngine;

namespace Systems.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public Sound[] musicSounds, sfxSounds, ambSounds, uiSounds;
        public AudioSource musicSource, sfxSource, ambSource, uiSource;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void PlayMusic(string clipName)
        {
            Sound s = Array.Find(musicSounds, x => x.clipName == clipName);

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

        public void PlaySfx(string clipName, float pitch)
        {
            Sound s2 = Array.Find(sfxSounds, x => x.clipName == clipName);

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
        
        public void PlaySfx(string clipName)
        {
            Sound s2 = Array.Find(sfxSounds, x => x.clipName == clipName);

            if (s2 == null)
            {
                Debug.Log("Sound Not Found");
            }

            else
            {
                sfxSource.PlayOneShot(s2.clip, s2.volume);
            }
        }

        public void PlayUI(string clipName)
        {
            Sound s3 = Array.Find(uiSounds, x => x.clipName == clipName);

            if (s3 == null)
            {
                Debug.Log("Sound Not Found");
            }

            else
            {
                uiSource.PlayOneShot(s3.clip, s3.volume);
            }
        }

        public void PlayAmb(string clipName)
        {
            Sound s4 = Array.Find(ambSounds, x => x.clipName == clipName);

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

        public void ToggleMusic()
        {
            musicSource.mute = !musicSource.mute;
        }
        public void ToggleSfx()
        {
            sfxSource.mute = !sfxSource.mute;
        }
    }
}
