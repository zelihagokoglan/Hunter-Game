// DoorController.cs
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Collider blocker;

    bool isOpen;
    public bool IsOpen => isOpen;

    void Awake() => SetOpen(false);

    void OnEnable()  => StaticEvents.AllEnemiesDead += Open;
    void OnDisable() => StaticEvents.AllEnemiesDead -= Open;

    void Start()
    {
        StartCoroutine(DelayedInitialCheck());
    }

    IEnumerator DelayedInitialCheck()
    {
        yield return null;                
        yield return new WaitForSeconds(0.05f);

        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"[DoorController] Initial Enemy count by TAG={enemyCount}");

        if (enemyCount == 0)
            Open();
    }

    void SetOpen(bool open)
    {
        isOpen = open;
        if (blocker) blocker.enabled = !open;
    }

    void Open()
    {
        if (isOpen) return;
        Debug.Log("[DoorController] OPEN");
        SetOpen(true);
    }
}