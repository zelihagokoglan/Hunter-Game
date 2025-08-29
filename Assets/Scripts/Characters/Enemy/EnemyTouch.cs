using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyTouch : MonoBehaviour
{
    public string playerTag = "Player";
    Transform player;
    
    public float moveSpeed = 2f;
    public float turnSpeedDeg = 540f;
    public float yLock = 0f;  

    public BoxCollider moveArea;              
    public float waypointReachDist = 0.5f;    
    public Vector2 pauseRange = new Vector2(0.5f, 1.5f); 

    public float chaseDistance = 7f;
    public float giveUpDistance = 10f;

    public int touchDamage = 10;
    public float damageCooldown = 1f;

    Vector3 wanderTarget;
    float pauseTimer = 0f;
    float lastDamageTime = -999f;

    enum State { Wander, Chase }
    State state = State.Wander;

    void Awake()
    {
        var p = GameObject.FindWithTag(playerTag);
        if (p) player = p.transform;
        PickNewWanderPoint();
    }

    void Update()
    {
        if (!player) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float dist = toPlayer.magnitude;

        if (state == State.Wander && dist <= chaseDistance) state = State.Chase;
        else if (state == State.Chase && dist >= giveUpDistance) state = State.Wander;

        if (state == State.Wander) UpdateWander();
        else UpdateChase(toPlayer);

        var pos = transform.position;
        pos.y = yLock;
        transform.position = pos;
    }
    void UpdateWander() // düşman rastgele bir noktaya doğru dolaşır
                        // bekleme süresi varsa bekle yoksa hedefe doğru hareket eder
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        MoveTowards(wanderTarget);

        if ((transform.position - wanderTarget).sqrMagnitude <= waypointReachDist * waypointReachDist)
        {
            pauseTimer = Random.Range(pauseRange.x, pauseRange.y);
            PickNewWanderPoint();
        }
    }

    void UpdateChase(Vector3 toPlayer) // Düşman, Player’a doğru yönelir.
                                       // Rotasyonunu yumuşak şekilde Player’a çevirir.
    {
        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            var look = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeedDeg * Time.deltaTime);
        }
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }

    void MoveTowards(Vector3 target) // Verilen hedef noktasına doğru yönelip hareket etmesini sağlar.
                                     // Rotasyonu hedef yönüne döndürür, ardından ileri doğru hareket ettirir.
    {
        Vector3 to = target - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;

        var look = Quaternion.LookRotation(to.normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeedDeg * Time.deltaTime);
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }

    void PickNewWanderPoint() // Düşmana yeni rastgele bir dolaşma hedefi seçtirir.
                              // Eğer moveArea (BoxCollider) atanmışsa → bu alanın içinde nokta seçer.
                              // Yoksa → mevcut konum çevresinde 3 birimlik daire içinde nokta seçer.
    {
        if (moveArea)
        {
            Bounds b = moveArea.bounds;
            float x = Random.Range(b.min.x, b.max.x);
            float z = Random.Range(b.min.z, b.max.z);
            wanderTarget = new Vector3(x, yLock, z);
        }
        else
        {
            // Alan atanmazsa, mevcut pozisyon çevresinde 3 birimlik dairede gez
            Vector2 r = Random.insideUnitCircle * 3f;
            wanderTarget = new Vector3(transform.position.x + r.x, yLock, transform.position.z + r.y);
        }
    }

    void OnCollisionEnter(Collision col) // Player ile ilk temas olduğunda hasar vermeye çalışır (TryDamage).
    {
        if (col.gameObject.CompareTag(playerTag)) TryDamage(col.gameObject);
    }

    void OnCollisionStay(Collision col) // Player ile çarpışma devam ediyorsa hasar vermeye çalışır.
    {
        if (col.gameObject.CompareTag(playerTag)) TryDamage(col.gameObject);
    }

    void OnTriggerEnter(Collider other) // Eğer düşmanın collider’ı Trigger modundaysa, Player ilk girdiğinde hasar verir.
    {
        if (other.CompareTag(playerTag)) TryDamage(other.gameObject);
    }

    void OnTriggerStay(Collider other) // Trigger alanında Player durmaya devam ederse hasar verir
    {
        if (other.CompareTag(playerTag)) TryDamage(other.gameObject);
    }

    void TryDamage(GameObject target) // Player’a hasar vermeyi dener.
                                      // Hasar verme, damageCooldown süresince bir kez yapılır 
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;

        Hp hp = target.GetComponent<Hp>();
        if (hp != null)
        {
            hp.TakeDamage(touchDamage);
        }
    }
}
