using System;
using TMPro;
using UnityEngine;

namespace Systems.Platforms
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public int points = 0;
        
        [Header("Text Elements")]
        [SerializeField] TextMeshProUGUI pointsText;

        private void Update()
        {
            pointsText.text = points.ToString();
        }

        public void AddPoints(int pointsToAdd)
        {
            points += pointsToAdd;
        }
    }
}
