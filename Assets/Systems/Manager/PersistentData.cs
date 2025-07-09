using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class PersistentData : Singleton<PersistentData>
    {
        public int currentScore;
        public int highScore;
        
        public void ResetScore()
        {
            currentScore = 0;
        }
        
        public void SaveHighScore()
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
        }
        
        public void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", PlayerPrefs.GetInt("HighScore", 0));
        }
    }
}