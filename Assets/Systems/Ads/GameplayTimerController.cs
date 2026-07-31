using UnityEngine;

public class GameplayTimerController : MonoBehaviour
{
    private float currentTime;
    private bool isGameplayRunning;

    public float CurrentTime => currentTime;

    private void FixedUpdate()
    {
        if(isGameplayRunning == true)
        {
            currentTime += Time.fixedDeltaTime;
        }
    }

    //public void StartOver()
    //{
    //    currentTime = 0f;
    //    SetIsRunning(true);
    //}

    public void SetIsRunning(bool isGameplayRunning)
    {
        this.isGameplayRunning = isGameplayRunning;
    }

    public void ResetTimer() => currentTime = 0f;
}
