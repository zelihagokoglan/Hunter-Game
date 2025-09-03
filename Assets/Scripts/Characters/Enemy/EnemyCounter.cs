using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    static EnemyCounter _instance;  // Sahnede tek olsun (isteğe bağlı)
    int alive;                      // Yaşayan düşman sayısı
    bool allRaised;                 // AllEnemiesDead sadece 1 kez yayılsın

    void Awake()
    {
        if (_instance && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        allRaised = false;
    }

    void OnEnable()  { StaticEvents.EnemyDied += OnEnemyDied; }
    void OnDisable() { StaticEvents.EnemyDied -= OnEnemyDied; }

    void Start()
    {
        // TAG ile sayım (Enemy tag’i tüm düşman prefablarında olmalı)
        alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"[EnemyCounter] Start alive(by TAG)={alive}");

        if (alive == 0) RaiseAllDead();
    }

    void OnEnemyDied()
    {
        if (allRaised) return;
        alive = Mathf.Max(0, alive - 1);
        Debug.Log($"[EnemyCounter] EnemyDied -> alive={alive}");
        if (alive == 0) RaiseAllDead();
    }

    void RaiseAllDead()
    {
        if (allRaised) return;
        allRaised = true;
        Debug.Log("[EnemyCounter] ALL DEAD -> RaiseAllEnemiesDead()");
        StaticEvents.RaiseAllEnemiesDead();
    }
}