// DoorController.cs
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Collider blocker;

    bool isOpen;
    public bool IsOpen => isOpen;

    void Awake() => SetOpen(false); // başta kapalı

    void OnEnable()  => StaticEvents.AllEnemiesDead += Open; // tüm düşmanlar öldüğünde bu kapı kendini açar.
    void OnDisable() => StaticEvents.AllEnemiesDead -= Open; // Script devre dışı bırakıldığında bu aboneliği kaldırır.
                                                             // Böylece gereksiz dinleme olmaz, memory leak önlenir.

    void Start()
    {
        StartCoroutine(DelayedInitialCheck()); // Oyun başladığında DelayedInitialCheck() coroutine’ini başlatır.
                                               // sahne başında düşman olup olmadığını kısa bir gecikmeyle kontrol eder.
    }

    IEnumerator DelayedInitialCheck()  // sahnedeki düşman sayısını bulur eğer hiç düşman yoksa kapıyı açar (Open()).
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