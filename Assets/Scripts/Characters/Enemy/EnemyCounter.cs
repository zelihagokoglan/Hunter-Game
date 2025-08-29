using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    static EnemyCounter _instance; // Scripti singleton yapar Yani sahnede sadece bir tane EnemyCounter olmasına izin verir.
    int alive; // sahnede yaşayan düşman sayısı                     // bu da manager içindedir tüm scenelerde  var 
    bool allRaised; // tüm düşmanların ölme olayı

    void Awake()
    {
        if (_instance && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        allRaised = false; // başlangıçta tüm düşmanlar öldü olarak setlenir
    }

    void OnEnable()  { StaticEvents.EnemyDied += OnEnemyDied; } // Bir düşman öldüğünde tetiklenen olaya abone olur.
    void OnDisable() { StaticEvents.EnemyDied -= OnEnemyDied; } // Script kapanınca bu olayı dinlemeyi bırakır.

    void Start()
    {
        alive = GameObject.FindGameObjectsWithTag("Enemy").Length; // Enemy tag’ine sahip kaç düşman olduğunu sayar.
        Debug.Log($"[EnemyCounter] Start alive(by TAG)={alive}");

        if (alive == 0) RaiseAllDead(); // Eğer düşman yoksa RaiseAllDead() çağırılır.
    }

    void OnEnemyDied() // Her düşman öldüğünde çalışır.
    {
        if (allRaised) return; 
        alive = Mathf.Max(0, alive - 1); // Düşman sayısını 1 azaltır, 0’ın altına düşmesini engeller.
        Debug.Log($"[EnemyCounter] EnemyDied -> alive={alive}");
        if (alive == 0) RaiseAllDead();
    }

    void RaiseAllDead()  // tüm düşmanların öldüğü bilgisini oyunun diğer sistemlerine duyurur
                         // kapı açma, level geçiş, skor artışı
    {
        if (allRaised) return;
        allRaised = true;
        Debug.Log("[EnemyCounter] ALL DEAD -> RaiseAllEnemiesDead()");
        StaticEvents.RaiseAllEnemiesDead();
    }
}