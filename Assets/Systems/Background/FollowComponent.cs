using UnityEngine;

namespace Systems.Background
{
    /// <summary>
    /// Su ÚNICA responsabilidad es hacer que este GameObject siga la posición de un target.
    /// </summary>
    public class FollowComponent : MonoBehaviour
    {
        [Header("Configuración de Seguimiento")]
        [SerializeField] private Transform target;
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;

        [Tooltip("Si está activo, solo actualizará la posición Y si la nueva es mayor a la anterior.")]
        [SerializeField] private bool biggestYPosition;

        private float _previousYPosition = float.MinValue;

        private void Start()
        {
            if (target == null)
            {
                Debug.LogError("El campo 'target' no está asignado en " + gameObject.name, this.gameObject);
                enabled = false;
            }
        }

        private void LateUpdate()
        {
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