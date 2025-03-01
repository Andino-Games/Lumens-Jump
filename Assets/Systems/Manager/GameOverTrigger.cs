using UnityEngine;
using UnityEngine.Events;

public class GameOverTrigger : MonoBehaviour
{
    public UnityEvent onGameOver;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onGameOver.Invoke();
        }
    }
}
