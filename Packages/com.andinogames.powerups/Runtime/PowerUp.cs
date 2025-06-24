using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Andino Games/PowerUps/Object PowerUp")]
public class PowerUp : MonoBehaviour
{
    public List<PowerUpComponentId> powerUpComponentIds = new List<PowerUpComponentId>();
    private Collider2D _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        if (!_collider)
        {
            throw new MissingComponentException("Collider2D not found on PowerUp GameObject. Please add a Collider2D component to the PowerUp GameObject.");
        }
        
        _collider.isTrigger = true;
    }
}