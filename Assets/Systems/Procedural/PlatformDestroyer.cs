using Systems.Platforms;
using UnityEngine;

namespace Systems.Procedural
{
    public class PlatformDestroyer : MonoBehaviour
    {
        [SerializeField] private LevelGenerator leveGen;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                Debug.Log("Destroying platform");

                GameObject platformObject = other.gameObject;

                platformObject.SetActive(false);
            
                Platform platform = platformObject.GetComponent<Platform>();
            
                platform?.ReleasePlatform();
            
                leveGen?.Spawn();
            }
        }
    }
}
