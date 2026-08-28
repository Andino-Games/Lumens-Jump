using TMPro;
using UnityEngine;

public class DeveloperUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameplayTimer; 

    public void UpdateGameplayTimer(float currentTime)
    {
        gameplayTimer.text = currentTime.ToString("0.0");
    }
}
