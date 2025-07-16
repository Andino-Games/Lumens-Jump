using System;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.UI.Cordel
{
    public class CordelBreakEventHandler : MonoBehaviour
    {
        public UnityEvent onCordelBreak;
        
        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            onCordelBreak.Invoke();
        }
    }
}