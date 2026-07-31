using System;
using UnityEngine;

namespace Systems.Level.Data
{
    [Serializable]
    public struct PlatformData
    {
        public Vector3 position;
        public bool hasBeenUsed;
    }
}