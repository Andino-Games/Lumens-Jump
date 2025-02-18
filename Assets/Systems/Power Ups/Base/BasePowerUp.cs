using UnityEngine;
using UnityEngine.Serialization;

public class BasePowerUp : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        Debug.Log($"This power up is of type: {EPowerUpType.Base}");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivatePowerUp();
        }
    }

    public virtual void ActivatePowerUp()
    {
        Debug.Log($"This PowerUp {EPowerUpType.Base} is being used}}");
    }
}

public enum EPowerUpType
{
    Base,
    Trampoline,
    Rocket,
    Zipline,
}
