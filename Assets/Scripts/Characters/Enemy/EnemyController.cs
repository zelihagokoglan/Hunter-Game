using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public string targetTag = "Player";

    public float moveSpeed = 2f;        
    public float turnSpeedDeg = 720f;   
    public float stoppingDistance = 1f; 

    public Transform Target => target;

    NavMeshAgent agent;
    Transform target;

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
        target = GameObject.FindWithTag(targetTag)?.transform;
    }

    void Update()
    {
        if (!target)
        {
            var go = GameObject.FindWithTag(targetTag);
            if (go) target = go.transform;
            return;
        }

        agent.SetDestination(target.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;         
            agent.velocity = Vector3.zero;  
            // TODO: Attack animasyonu/logic
        }
        else
        {
            agent.isStopped = false;
        }
    }
    void OnDisable()
    {
        if (agent) agent.ResetPath();
    }
}