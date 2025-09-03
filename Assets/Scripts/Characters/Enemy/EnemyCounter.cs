using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    static EnemyCounter _instance;  
    int alive;                      
    bool allRaised;                 

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