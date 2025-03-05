using UnityEngine;

namespace Systems.Platforms
{
    public class PlatformContact : MonoBehaviour
    {
        private bool canGivePoints = true; // Ahora inicia en true para permitir sumar puntos
        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && canGivePoints)
            {
                AddPoints();
                Debug.Log("Puntos sumados" + gameManager.points);
            }
        }

        private void AddPoints()
        {
            gameManager.AddPoints(1); // Suma 1 punto en el GameManager
            canGivePoints = false; // Evita que la plataforma siga dando puntos repetidamente
        }
    }
}