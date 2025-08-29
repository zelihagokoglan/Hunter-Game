using System.Collections;
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent kullanıyorsan

public class EnemyKamikazeExploder : MonoBehaviour
{
    public string targetTag = "Player";
    Transform target;
    
    public float explodeRange = 1.6f;   // bu mesafede patla
    public float windupTime   = 0.25f;  // patlamadan önce kısa uyarı

    public int damage   = 30;
    public float radius = 2.5f;
    public LayerMask playerMask;        // sadece Player layer
    public bool destroySelfAfter = true;

    public GameObject explosionVFX;     // Particle (Stop Action=Destroy)
    public AudioSource explosionSfx;    // aynı objede bir AudioSource

    public bool stopAgentOnExplode = true;

    bool arming;        // windup sırasında
    bool done;          // patladı/öldü
    bool hasExploded;   // tek sefer güvenliği
    NavMeshAgent agent; // varsa dursun

    void Awake()
    {
        target = GameObject.FindWithTag(targetTag)?.transform; // player ı bulur
        agent  = GetComponent<NavMeshAgent>(); // navmesh agent varsa referans alır
    }

    void Update()
    {
        if (done || arming || !target) return;
// done: düşman patlamışsa arming: hazırlık sürecinde veya target yoksa hiçbir şey yapmaz
        Vector3 to = target.position - transform.position;
        to.y = 0f;

        if (to.magnitude <= explodeRange)
            StartCoroutine(ArmAndExplode());
    }

    IEnumerator ArmAndExplode()
    {
        arming = true;

        // enemynin patlama için bekleme süresi kırmızı yancak 
        yield return new WaitForSeconds(windupTime);

        ExplodeNow();
    }
    
    public void TriggerExplosion()
    {
        ExplodeNow();
    }

    void ExplodeNow()
    {
        Debug.Log("Patlama oldu!");

        if (hasExploded) return;
        hasExploded = true; // patlama birden fazla çalışmasın
        done = true; // patlama tamamlandı

        if (stopAgentOnExplode && agent)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        // VFX/SFX
        if (explosionVFX) Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if (explosionSfx) explosionSfx.Play();

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerMask, QueryTriggerInteraction.Ignore);
        foreach (var c in hits)
        {
            Hp hp = c.GetComponentInParent<Hp>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                Debug.Log($"Hasar verildi: {hp.name}");
            }
        }

        if (destroySelfAfter)
            Destroy(gameObject);
    }


    void OnDrawGizmosSelected() // sahnede patlama alanını örsel olarak göstermek için kullanıldı
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, radius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}
