using Systems.UI.MouseClick;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.UI.Utils
{
    public class SimpleButton : MonoBehaviour, IClickDown
    {
        [SerializeField] private bool oneUse;
        
        public UnityEvent onClickEvent;
        
        private bool _isUsed;
        
        public void OnClick()
        {
            if (oneUse && _isUsed)
            {
                return;
            }
            onClickEvent.Invoke();
            _isUsed = true;
        }
    }
}