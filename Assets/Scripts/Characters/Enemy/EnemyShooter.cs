using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyShooter : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public float fireInterval = 4f;
    public float maxAcquireRange = 100f;

    EnemyController controller;
    float fireTimer;

    void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    void Start()
    {
        fireTimer = fireInterval;
    }

    void Update()
    {
        var target = controller.Target;
        if (target == null || firePoint == null || projectilePrefab == null) return;

        Vector3 aimPoint = GetTargetPoint(target);
        if (Vector3.Distance(transform.position, aimPoint) > maxAcquireRange) return;

        Vector3 dir = (aimPoint - firePoint.position).normalized;
        if (dir.sqrMagnitude < 0.0001f) return;

        firePoint.rotation = Quaternion.LookRotation(dir, Vector3.up);

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.FireAt(target, dir, new Vector3(0f, 180f, 0f), "Enemy");

            var killer = proj.GetComponent<DestroyOnObstacleHit>();
            if (killer) killer.Init("Enemy");

        }
    }

    Vector3 GetTargetPoint(Transform t)
    {
        if (t.TryGetComponent<Collider>(out var col))
            return col.bounds.center;
        return t.position;
    }
}