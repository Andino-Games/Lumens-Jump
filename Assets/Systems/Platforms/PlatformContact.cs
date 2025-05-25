using UnityEngine;

namespace Systems.Platforms
{
    public class PlatformContact : MonoBehaviour
    {
        private bool canGivePoints = true;
        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
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
            gameManager.AddPoints(1);
            canGivePoints = false;
        }
    }
}