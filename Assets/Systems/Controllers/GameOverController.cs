using Systems.Manager;
using Systems.UI.MouseClick;
using TMPro;
using UnityEngine;
using System;

namespace Systems.UI
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;

        [Header("Panels")]
        [SerializeField] private GameObject gameOverPanel;

        private void Start()
        {
            ShowFinalScore();

            MouseClicks.Instance.gameObject.SetActive(true);
        }

        public void ShowFinalScore()
        {
            HidePanels();

            if (gameOverPanel)
            {
                gameOverPanel.SetActive(true);
            }
            if (finalScoreText)
            {
                finalScoreText.text = PersistentData.Instance.CurrentScore.ToString();
            }
            if (highScoreText)
            {
                highScoreText.text = PersistentData.Instance.HighScore.ToString();
            }
        }

        private void HidePanels()
        {
            gameOverPanel?.SetActive(false);
        }
    }
}