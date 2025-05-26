using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance;

    public int currentScore;
    public int highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //  Aseguramos que este objeto persista
        }
        else
        {
            Destroy(gameObject);  //  Evitamos duplicados
        }
    }

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