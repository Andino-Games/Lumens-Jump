using Systems.Manager;
using TMPro;
using UnityEngine;

namespace Systems.UI
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        private void Start()
        {
            ShowFinalScore();
        }

        private void ShowFinalScore()
        {
            if (finalScoreText)
            {
                finalScoreText.text = "Final Score: " + PersistentData.Instance.currentScore;
            }
            if (highScoreText)
            {
                highScoreText.text = "High Score: " + PersistentData.Instance.highScore;
            }
        }
    }
}