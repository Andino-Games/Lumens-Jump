using UnityEngine;

namespace Systems.UI.Cordel
{
    public class CordelDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(other.gameObject);
        }
    }
}