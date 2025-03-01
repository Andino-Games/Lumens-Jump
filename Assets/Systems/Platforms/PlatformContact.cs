using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Platforms
{
    public class PlatformContact : Platform
    {
        private bool canGivePoints;
        
        private GameManager gameManager;

        public override void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            
            
        }

        public void AddPoints()
        {
            if (canGivePoints)
            {
                gameManager.AddPoints(1);
            }
            
            canGivePoints = false;
        }
    }
}
