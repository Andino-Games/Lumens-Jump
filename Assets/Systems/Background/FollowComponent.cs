using UnityEngine;

namespace Systems.Background
{
    public class FollowComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        [SerializeField] private bool biggestYPosition = false;

        private float _previousYPosition;
        
        private void LateUpdate()
        {
            if (!target) return;

            Vector3 newPosition = transform.position;

            if (followX)
            {
                newPosition.x = target.position.x;
            }

            if (followY)
            {
                if (biggestYPosition && target.position.y < _previousYPosition)
                {
                    newPosition.y = _previousYPosition;
                }
                else
                {
                    newPosition.y = target.position.y;
                    _previousYPosition = target.position.y;
                }
            }

            transform.position = newPosition;
        }
    }
}