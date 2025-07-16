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
        
        public int LoadHighScore()
        {
            return PlayerPrefs.GetInt("Score", 0);
        }
        
        public void ResetScore() => _currentScore = 0;
        
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
        }
    }
}