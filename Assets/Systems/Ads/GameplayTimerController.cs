using System;
using UnityEngine;

public class GameplayTimerController : MonoBehaviour
{
    private float currentTime;
    private bool isGameplayRunning;

    public Action<float> OnCurrentTimeUpdate;

    public float CurrentTime => currentTime;

    private void FixedUpdate()
    {
        if(isGameplayRunning == true)
        {
            currentTime += Time.fixedUnscaledDeltaTime;
            OnCurrentTimeUpdate?.Invoke(currentTime);
        }
    }

    public void SetIsRunning(bool isGameplayRunning)
    {
        this.isGameplayRunning = isGameplayRunning;
    }

    public void ResetTimer() => currentTime = 0f;
}
