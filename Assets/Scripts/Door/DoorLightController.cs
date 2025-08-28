using UnityEngine;

public class DoorLightController : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] Light doorLight;     
    [SerializeField] DoorController door; 

    void Awake()
    {
        if (doorLight != null)
            doorLight.enabled = false; 
    }

    void OnEnable()
    {
        StaticEvents.AllEnemiesDead += OnAllEnemiesDead;
        Debug.Log("[DoorLight] Subscribed AllEnemiesDead.");
    }

    void OnDisable()
    {
        StaticEvents.AllEnemiesDead -= OnAllEnemiesDead;
        Debug.Log("[DoorLight] Unsubscribed AllEnemiesDead.");
    }

    void OnAllEnemiesDead()
    {
        if (doorLight != null)
        {
            doorLight.enabled = true;
            Debug.Log("[DoorLight] AllEnemiesDead -> Light ON (event).");
        }
        else
        {
            Debug.LogWarning("[DoorLight] AllEnemiesDead geldi ama doorLight atanmadı!");
        }
    }

    void Update()
    {
        if (door && door.IsOpen && doorLight && !doorLight.enabled)
        {
            doorLight.enabled = true;
            Debug.Log("[DoorLight] Door.IsOpen==true -> Light ON (safety).");
        }
    }

    public void TurnOffLight()
    {
        if (doorLight != null)
        {
            doorLight.enabled = false;
            Debug.Log("[DoorLight] TurnOffLight() -> Light OFF.");
        }
    }

    public void TurnOnLight()
    {
        if (doorLight != null)
        {
            doorLight.enabled = true;
            Debug.Log("[DoorLight] TurnOnLight() -> Light ON (direct).");
        }
    }
}