using System;
using Systems.Manager;
using Systems.UI.MouseClick;
using TMPro;
using UnityEngine;

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
        }
    }
}