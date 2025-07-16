using System.Collections.Generic;
using Systems.Level.Data;
using UnityEngine;

namespace Systems.Level.Platforms
{
    public class PlatformMoving : Platform
    {
        [Header("Moving Platform")]
        [SerializeField] private List<Transform> points;
        [SerializeField] private Transform platform;
        [SerializeField] private float moveSpeed = 2;
        
        [Header("Velocity Increase")]
        [SerializeField] private float timeToIncreaseSpeed = 12f;
        [SerializeField] private float currentTime = 1f;
        [SerializeField] private float speedIncrease = 1.1f;

        private Vector3 _destinationPosition;
        private int _goalPoint;
        private bool _canMove;

        #region Platform Methods

        protected override void OnCreatedPlatform(PlatformData platformData)
        {
            base.OnCreatedPlatform(platformData);
            _destinationPosition = platformData.position;

            
            transform.position = new Vector2(0, platformData.position.y);
            
            int randomSign = Random.Range(0, 2) == 0 ? -1 : 1;
            platform.localPosition = new Vector2(platformData.position.x, 0) + new Vector2(randomSign * 6, 0);
            
            LeanTween.move(platform.gameObject, _destinationPosition, 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() => _canMove = true);
        }

        protected override void OnUpdatePlatform()
        {
            base.OnUpdatePlatform();
            if (!_canMove)
            {
                return;
            }
            
            currentTime += Time.deltaTime;
            if (currentTime >= timeToIncreaseSpeed)
            {
                moveSpeed *= speedIncrease;
                currentTime = 0f;
            }
            MoveToNextPoint();
        }

        public override void SetPowerUp(Transform powerUp)
        {
            base.SetPowerUp(powerUp);
            
            powerUp.SetParent(platform);
            powerUp.localPosition = new Vector3(0, 0.5f, 0);
        }

        #endregion

        void MoveToNextPoint()
        {
            platform.position = Vector2.MoveTowards(platform.position, points[_goalPoint].position,Time.deltaTime*moveSpeed);
            if(Vector2.Distance(platform.position, points[_goalPoint].position) < 0.1f)
            {
                if (_goalPoint == points.Count - 1)
                    _goalPoint = 0;
                else
                    _goalPoint++;
            }
        }
    }
}
