using Systems.Utils;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems.Manager
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineCamera defaultCamera;
        [SerializeField] private CinemachineCamera effectCamera;

        private void Start()
        {
            if (!defaultCamera || !effectCamera)
            {
                Debug.LogError("CineMachine cameras are not assigned in the CameraManager.");
            }
        }

        public void SetDefaultCamera()
        {
            defaultCamera.Priority = 10;
            effectCamera.Priority = 0;
        }
        
        public void SetEffectCamera()
        {
            defaultCamera.Priority = 0;
            effectCamera.Priority = 10;
        }
        
    }
}