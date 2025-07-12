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
        }

        public void SetCamera(string cameraName)
        {
            foreach (CamerasType cameraType in cameras)
            {
                if (cameraType.cameraName.Equals(cameraName, StringComparison.OrdinalIgnoreCase))
                {
                    cameraType.camera.gameObject.SetActive(true);
                    continue;
                }
                
                cameraType.camera.gameObject.SetActive(false);
            }
        }
        
    }
}