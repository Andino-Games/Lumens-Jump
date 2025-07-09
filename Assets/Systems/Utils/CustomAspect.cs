using UnityEngine;

namespace Systems.Utils
{
    public class CustomAspect : MonoBehaviour
    {    
        public enum ReferenceMode { DesignedAspectRatio, OriginalResolution };

        public Color matteColor = new Color(0, 0, 0, 1);
        public ReferenceMode referenceMode; 
        public float x=16;
        public float y=9;  
        public float width = 960;
        public float height = 540;
        public bool onAwake = true;
        public bool onUpdate = true;

        private Camera _cam;
        private Camera _letterBoxerCamera;

        public void Awake()
        {
            // store reference to the camera
            _cam = GetComponent<Camera>();

            // add the letterboxing camera
            AddLetterBoxingCamera();

            // perform sizing if onAwake is set
            if (onAwake)
            {
                PerformSizing();
            }
        }

        public void Update()
        {
            // perform sizing if onUpdate is set
            if (onUpdate)
            {
                PerformSizing();
            }
        }

        private void OnValidate()
        {
            x = Mathf.Max(1, x);
            y = Mathf.Max(1, y);
            width = Mathf.Max(1, width);
            height = Mathf.Max(1, height);
        }

        private void AddLetterBoxingCamera()
        {
            // check that we don't have a camera already at -100 (the lowest depth) which will cause issues
            Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (Camera currentCamera in allCameras)
            {             
                if (Mathf.Approximately(currentCamera.depth, -100))
                {
                    Debug.LogError("Found " + currentCamera.name + " with a depth of -100. Will cause letter boxing issues. Please increase it's depth.");
                }
            }

            // create a camera to render the background used for matte bars
            _letterBoxerCamera = new GameObject().AddComponent<Camera>();
            _letterBoxerCamera.backgroundColor = matteColor;
            _letterBoxerCamera.cullingMask = 0;
            _letterBoxerCamera.depth = -100;
            _letterBoxerCamera.farClipPlane = 1;
            _letterBoxerCamera.useOcclusionCulling = false;
            _letterBoxerCamera.allowHDR = false;
            _letterBoxerCamera.allowMSAA = false;
            _letterBoxerCamera.clearFlags = CameraClearFlags.Color;
            _letterBoxerCamera.name = "Letter Boxer Camera";        
        }

        private void PerformSizing()
        {
            float targetRatio = x / y;

            // recalc if using resolution as reference
            if (referenceMode == ReferenceMode.OriginalResolution)
            {
                targetRatio = width / height;
            }

            // determine the game window's current aspect ratio
            float windowAspect = (float) Screen.width / Screen.height;

            // this amount should scale the current viewport height
            float scaleHeight = windowAspect / targetRatio;

            // if the scaled height is less than current height, add letterbox
            if (scaleHeight < 1.0f)
            {
                Rect rect = _cam.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                _cam.rect = rect;
            }
            else // add pillar box
            {
                float scaleWidth = 1.0f / scaleHeight;

                Rect rect = _cam.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                _cam.rect = rect;
            }
        }
    }
}