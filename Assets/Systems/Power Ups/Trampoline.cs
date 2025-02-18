using UnityEngine;

public class Trampoline : BasePowerUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        Debug.Log($"This PowerUp is: {EPowerUpType.Trampoline}");
    }

    public override void ActivatePowerUp()
    {
        Debug.Log($"The PowerUp {EPowerUpType.Trampoline} is being used");
    }
}
