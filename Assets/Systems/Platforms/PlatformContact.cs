using Systems.Manager;
using UnityEngine;

namespace Systems.Platforms
{
    public class PlatformContact : MonoBehaviour
    {
        private bool _canGivePoints;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            // Esto reinicia el estado de la plataforma cada vez que se activa desde el pool.
            _canGivePoints = true;
        }

        public void GrantPoints()
        {
            if (!_canGivePoints) return;
            
            _gameManager.AddPoints(1);
            _canGivePoints = false;
        }
    }
}