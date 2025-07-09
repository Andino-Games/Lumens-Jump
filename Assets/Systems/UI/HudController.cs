using Systems.Manager;
using TMPro;
using UnityEngine;

namespace Systems.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pausePanel;
        
        private void Update()
        {
            if (pointsText)
            {
                pointsText.text = PersistentData.Instance.currentScore.ToString();
            }
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

        public void PausePanel()
        {
            if (pausePanel)
            {
                pausePanel.SetActive(true);
            }
        }

        public void ResumeGame()
        {
            if (pausePanel)
            {
                pausePanel.SetActive(false);
            }
        }

    }
}