using UnityEngine;

[DefaultExecutionOrder(100)] // Projectile.Update'tan sonra çalışsın
public class DestroyOnObstacleHit : MonoBehaviour
{
    public LayerMask obstacleMask;      
    public float sweepRadius = 0.08f;   
    public int warmupFrames = 1;        
    public bool includeTriggersInSweep = true; 

    string _ownerTag;                   
    Vector3 _lastPos;
    float _autoRadius = -1f;

    public void Init(string ownerTag) => _ownerTag = ownerTag;

    void OnEnable()
    {
        _lastPos = transform.position;
        
        if (TryGetComponent<SphereCollider>(out var sc))
        {
            float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            _autoRadius = sc.radius * scale * 0.9f; 
        }
    }

    void LateUpdate()
    {
        if (warmupFrames-- > 0) { _lastPos = transform.position; return; }

        Vector3 now = transform.position;
        Vector3 delta = now - _lastPos;
        float dist = delta.magnitude;

        if (dist > 0.0001f)
        {
            float r = (_autoRadius > 0f) ? _autoRadius : sweepRadius;
            var qti = includeTriggersInSweep ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            if (Physics.SphereCast(_lastPos, r, delta.normalized, out RaycastHit hit, dist, obstacleMask, qti))
            {
                var col = hit.collider;
                if (!ShouldIgnore(col))
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        _lastPos = now;
    }

    void OnCollisionEnter(Collision c)
    {
        if (IsObstacle(c.collider) && !ShouldIgnore(c.collider))
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsObstacle(other) && !ShouldIgnore(other))
            Destroy(gameObject);
    }

    bool IsObstacle(Collider col) => (obstacleMask.value & (1 << col.gameObject.layer)) != 0;

    bool ShouldIgnore(Collider col)
    {
        if (!string.IsNullOrEmpty(_ownerTag) && col.CompareTag(_ownerTag)) return true;
        if (col.CompareTag("Player") || col.CompareTag("Enemy")) return true;
        return false;
    }
}
