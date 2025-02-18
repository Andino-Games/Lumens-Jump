using UnityEngine;
using UnityEngine.Serialization;

public class BasePowerUp : MonoBehaviour
{
    private string powerUpMessage = "The Power Up is Used";
    
    public EPowerUpType PowerUpType;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"This power up is of type: {PowerUpType}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivatePowerUp();
        }
    }

    public virtual void ActivatePowerUp()
    {
        Debug.Log(powerUpMessage);
    }
}

public enum EPowerUpType
{
    Base,
    Trampoline,
    Rocket,
    Zipline,
}
