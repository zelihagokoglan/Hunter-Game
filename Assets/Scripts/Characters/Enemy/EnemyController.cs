using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // attribute, bu scripte bağlı enemy e navmeshagent ekler
public class EnemyController : MonoBehaviour
{
    public string targetTag = "Player";

    public float moveSpeed = 2f;        
    public float turnSpeedDeg = 720f;   
    public float stoppingDistance = 1f; 

    public Transform Target => target;

    NavMeshAgent agent; // Hareket ve yol bulma işlerini yapan Unity’nin AI componenti.
    Transform target; // player ın transformunu tutar

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.stoppingDistance = stoppingDistance;     
        agent.speed           = moveSpeed;          
        agent.angularSpeed    = turnSpeedDeg;         
        agent.acceleration    = 12f;                   
        agent.autoBraking     = true;                
        agent.updateRotation  = true;                  
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    void Start()
    {
        target = GameObject.FindWithTag(targetTag)?.transform; // sahnedeki player ı bulur ve hedef olarak atar
    }

    void Update()
    {
        if (!target)
        {
            var go = GameObject.FindWithTag(targetTag);
            if (go) target = go.transform;
            return;
        }

        agent.SetDestination(target.position);// hedefin pozisyonuna doğru gitmesini sağlar

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) // hedefe ulaştı mı kontrolü
        {
            agent.isStopped = true;         
            agent.velocity = Vector3.zero;  
            // TODO: Attack animasyonu/logic
        }
        else
        {
            agent.isStopped = false; // eğer hedefe ulaşılmadıysa hareket devam eder
        }
    }
    void OnDisable()
    {
        if (agent) agent.ResetPath(); // gameobject kapanınca agent yolu sıfırlanır
    }
}