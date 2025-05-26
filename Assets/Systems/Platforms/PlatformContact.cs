using UnityEngine;

namespace Systems.Platforms
{
    public class PlatformContact : MonoBehaviour
    {
        private bool _canGivePoints = true;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _canGivePoints)
            {
                AddPoints();
                Debug.Log("Puntos sumados " + PersistentData.Instance.currentScore);
            }
        }

        private void AddPoints()
        {
            _gameManager.AddPoints(1);
            _canGivePoints = false;
        }
    }
}