using System;
using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class PersistentData : Singleton<PersistentData>
    {
        public Action<int> OnCurrentScoreChanged;
        
        private int _currentScore;
        private int _highScore;

        public int CurrentScore => _currentScore;
        public int HighScore => _highScore;
        
        public int AddPoints()
        {
            _currentScore++;
            OnCurrentScoreChanged?.Invoke(_currentScore);
            return _currentScore;
        }
        
        
        /// Carga el récord guardado localmente y actualiza la variable interna.
        
        public int LoadHighScore()
        {
            
            _highScore = PlayerPrefs.GetInt("Score", 0);
            return _highScore;
        }
        
        public void ResetScore() => _currentScore = 0;
        
        
        /// Compara el puntaje actual con el récord y guarda si es mayor.
        
        public void SaveHighScore()
        {
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                PlayerPrefs.SetInt("Score", _currentScore);
                PlayerPrefs.Save();
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            // Inicializamos la carga de datos al arrancar el sistema.
            LoadHighScore();
        }
    }
}