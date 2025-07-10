using Systems.Manager;
using TMPro;
using UnityEngine;

namespace Systems.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pausePanel;
        
        private void Update()
        {
            if (pointsText)
            {
                pointsText.text = PersistentData.Instance.currentScore.ToString();
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