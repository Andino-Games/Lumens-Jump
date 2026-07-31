using UnityEngine;

namespace Systems.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string clipName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(1f, 5f)] public float pitch = 1f;
        public bool loop;
    }
}
