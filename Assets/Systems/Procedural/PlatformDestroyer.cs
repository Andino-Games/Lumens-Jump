using Systems.Platforms;
using UnityEngine;

namespace Systems.Procedural
{
    public class PlatformDestroyer : MonoBehaviour
    {
        [SerializeField] private LevelGenerator leveGen;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Lógica simple y directa: si una plataforma entra aquí, se recicla.
            if (other.CompareTag("Platform"))
            {
                Platform platform = other.GetComponent<Platform>();
            
                // Llama al método para liberar la plataforma de vuelta al pool.
                platform?.ReleasePlatform();
            
                // Pide al generador que cree una nueva plataforma arriba.
                leveGen?.Spawn();
            }
        }
    }
}