using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public int points = 0;

        public float initialDelay = 5f;

        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI pointsText;

        [SerializeField] GameObject gameOverPanel;

        [Header("InitialGround")]
        [SerializeField] private GameObject initialGround;

        

        private void Start()
        {
            StartCoroutine(GroundStart());
            gameOverPanel.SetActive(false);
        }

        private void Update()
        {
            pointsText.text = points.ToString();
        }

        public void AddPoints(int pointsToAdd)
        {
            points += pointsToAdd;
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
        }

        private IEnumerator GroundStart()
        {
            yield return new WaitForSeconds(initialDelay);

            initialGround.SetActive(false);
        }
    }
}
