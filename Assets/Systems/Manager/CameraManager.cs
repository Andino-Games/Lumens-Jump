using System;
using System.Collections.Generic;
using Systems.Utils;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.Manager
{
    [Serializable]
    public class CamerasType
    {
        public string cameraName;
        public CinemachineCamera camera;
    }
    
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private List<CamerasType> cameras;
        
        // Se eliminan las variables de movimiento ascendente de la cámara de aquí,
        // ya que esa lógica se ha movido a PlayerJump.cs
        // [Header("Upward Camera Movement")]
        // [SerializeField] private Transform _cameraFollowTarget; 
        // [SerializeField] private float _upwardMoveSpeed = 0.5f; 
        // [SerializeField] private float _deathOffset = 5f; 

        // private float _initialCameraFollowTargetY; 
        // private float _currentCameraTargetY; 
        
        private void Start()
        {
            if (cameras == null || cameras.Count == 0)
            {
                Debug.LogError("No cameras assigned in CameraManager.");
                return;
            }

            foreach (CamerasType cameraType in cameras)
            {
                if (!cameraType.camera)
                {
                    Debug.LogError($"Camera {cameraType.cameraName} is not assigned.");
                }
            }

            // La inicialización y el seguimiento del objetivo de la cámara
            // ahora se gestionan en PlayerJump.cs
            // if (_cameraFollowTarget == null)
            // {
            //     Debug.LogError("Camera Follow Target is not assigned in CameraManager. Please assign a Transform.");
            //     return;
            // }
            // _initialCameraFollowTargetY = _cameraFollowTarget.position.y;
            // _currentCameraTargetY = _initialCameraFollowTargetY;

            // Solo activamos la cámara por defecto
            SetCamera("Default"); 
            // La asignación del Follow de Cinemachine se hará en PlayerJump.cs
            // foreach (CamerasType cameraType in cameras)
            // {
            //     if (cameraType.cameraName.Equals("Default", StringComparison.OrdinalIgnoreCase))
            //     {
            //         cameraType.camera.Follow = _cameraFollowTarget;
            //         break;
            //     }
            // }
        }

        // Se elimina el método Update() de aquí, ya que el movimiento de la cámara
        // ahora se gestiona en PlayerJump.cs
        // private void Update()
        // {
        //     float newTargetY = _currentCameraTargetY + _upwardMoveSpeed * Time.deltaTime;
        //     _currentCameraTargetY = Mathf.Max(_currentCameraTargetY, newTargetY);
        //     _cameraFollowTarget.position = new Vector3(_cameraFollowTarget.position.x, _currentCameraTargetY, _cameraFollowTarget.position.z);
        // }

        public void SetCamera(string cameraName)
        {
            foreach (CamerasType cameraType in cameras)
            {
                if (cameraType.cameraName.Equals(cameraName, StringComparison.OrdinalIgnoreCase))
                {
                    cameraType.camera.gameObject.SetActive(true);
                    // La asignación del Follow de Cinemachine se hará en PlayerJump.cs
                    // if (cameraName.Equals("Default", StringComparison.OrdinalIgnoreCase))
                    // {
                    //     cameraType.camera.Follow = _cameraFollowTarget;
                    // }
                    continue;
                }
                
                cameraType.camera.gameObject.SetActive(false);
            }
        }

        // Se eliminan los métodos GetCameraY(), GetDeathOffset() y ResetCameraPosition()
        // ya que la lógica de Game Over y el control de la posición de la cámara
        // se gestionan ahora en PlayerJump.cs y el nuevo RisingDeathZone.
        // /// <summary>
        // /// Retorna la posición Y actual del objetivo de la cámara.
        // /// </summary>
        // public float GetCameraY()
        // {
        //     return _currentCameraTargetY;
        // }

        // /// <summary>
        // /// Retorna el offset de muerte por debajo de la cámara.
        // /// </summary>
        // public float GetDeathOffset()
        // {
        //     return _deathOffset;
        // }

        // /// <summary>
        // /// Resetea la posición del objetivo de la cámara a su valor inicial.
        // /// Útil al reiniciar el juego.
        // /// </summary>
        // public void ResetCameraPosition()
        // {
        //     if (_cameraFollowTarget != null)
        //     {
        //         _cameraFollowTarget.position = new Vector3(_cameraFollowTarget.position.x, _initialCameraFollowTargetY, _cameraFollowTarget.position.z);
        //         _currentCameraTargetY = _initialCameraFollowTargetY;
        //     }
        // }
    }
}
