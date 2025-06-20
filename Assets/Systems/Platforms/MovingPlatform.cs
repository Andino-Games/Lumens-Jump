using System.Collections.Generic;
using UnityEngine;

namespace Systems.Platforms
{
    public class MovingPlatform : Platform
    {
        public List<Transform> points;
        public Transform platform;
        int goalPoint = 0;
        public float moveSpeed = 2;

        [Header("CONTROL DE VELOCIDAD DE LA PLATAFORMA")]
        [SerializeField] private float timeToIncreaseSpeed = 12f;
        [SerializeField] private float currentTime = 0f;
        [SerializeField] private float speedIncrease = 1.1f;

        private void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeToIncreaseSpeed)
            {
                moveSpeed *= speedIncrease;
                currentTime = 0f; // Resetea el temporizador
            }
            MoveToNextPoint();
        }

        void MoveToNextPoint()
        {
            //Cambia la posicion de la plataforma
            platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position,Time.deltaTime*moveSpeed);
            if(Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
            {
                if (goalPoint == points.Count - 1)
                    goalPoint = 0;
                else
                    goalPoint++;
            }
        }
    }
}
