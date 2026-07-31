using System;
using Systems.Manager;
using Systems.UI.MouseClick;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI
{
    public class HudController : MonoBehaviour
    {
        private static readonly int AddPoints = Animator.StringToHash("AddPoints");

        [Header("Points Elements")]
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Animator pointsTextAnimator;

        [Header("Panels")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject joystickPanel;
        [SerializeField] private GameObject revivePanel;
        [SerializeField] private GameObject pausePanel;

        [Header("Pause")]
        [SerializeField] private Image pause;
        [SerializeField] private Color unactiveColor;
        [SerializeField] private Color activeColor;

        private void Start()
        {
            MouseClicks.Instance.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PersistentData.Instance.OnCurrentScoreChanged += UpdatePointsText;
        }

        private void OnDisable()
        {
            PersistentData.Instance.OnCurrentScoreChanged -= UpdatePointsText;
        }

        private void UpdatePointsText(int value)
        {
            if (pointsText)
            {
                pointsTextAnimator.SetTrigger(AddPoints);
                pointsText.text = value.ToString();
            }
        }

        public void SetRevivePanelActive(bool newActive)
        {
            revivePanel.SetActive(newActive);
            gameplayPanel.SetActive(!newActive);
            joystickPanel.SetActive(!newActive);
            pausePanel.SetActive(false);
        }

        public void SetPause(bool? newActive)
        {
            var b = pause.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (newActive == true)
            {
                b.color = unactiveColor;
                b.text = ">";
                pause.color = activeColor;

                pausePanel.SetActive(true);
                pause.gameObject.SetActive(false);
            }
            else if(newActive == false)
            {
                b.color = activeColor;
                b.text = "ll";
                pause.color = unactiveColor;

                pausePanel.SetActive(false);
                pause.gameObject.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(false);
                pause.gameObject.SetActive(false);
            }
        }
    }
}