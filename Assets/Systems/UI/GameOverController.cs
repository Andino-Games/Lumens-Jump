using Systems.Manager;
using Systems.UI.MouseClick;
using TMPro;
using UnityEngine;

namespace Systems.UI
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;

        private void Start()
        {
            ShowFinalScore();
            MouseClicks.Instance.gameObject.SetActive(true);
        }

        private void ShowFinalScore()
        {
            if (finalScoreText)
            {
                finalScoreText.text = PersistentData.Instance.CurrentScore.ToString();
            }
            if (highScoreText)
            {
                highScoreText.text = PersistentData.Instance.HighScore.ToString();
            }
        }
    }
}