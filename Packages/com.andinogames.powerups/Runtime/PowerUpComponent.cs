using System.Collections;
using UnityEngine;

public abstract class PowerUpComponent : MonoBehaviour
{
    /// <summary>
    /// GameObject representativo deal jugador.
    /// </summary>
    [HideInInspector] public GameObject player;
    public PowerUpComponentId powerUpComponentId;
    public bool isEnabled = true;

    #region Unity Methods

    private void Awake()
    {
        player = gameObject;
    }

    private void Start()
    {
        SetUpComponents();
    }

    public void ExecutePowerUp()
    {
        if (isEnabled)
        {
            StartCoroutine(Execute());
        }
    }

    #endregion

    public abstract void SetUpComponents();
    public abstract IEnumerator Execute();
}