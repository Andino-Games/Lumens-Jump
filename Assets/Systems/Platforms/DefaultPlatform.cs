using UnityEngine;

namespace Systems.Platforms
{
    public class DefaultPlatform : Platform
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void Start()
        {
            base.Start();
            //Default
            Debug.Log("DefaultPlatform");
        }
    }
}
