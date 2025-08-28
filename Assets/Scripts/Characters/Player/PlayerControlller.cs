using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Collider playArea; // Ground
    public Joystick joystick; // Canvas'taki (FloatingJoystickAutoHide) referansı

    Rigidbody rb;
    Vector2 clampX, clampZ;
    float halfX, halfZ;
    Vector3 inputDir;
    const float skin = 0.01f;   // duvara değmeden önce durma payı
    const float DEADZONE = 0.1f; // joystick gürültüsü için eşik

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Kayma engellemek için drag ( karakter ground üzerinde duramadığı için kayıyordu)
        rb.drag = 5f;

        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        // Boyut -> clamp payı
        if (TryGetComponent<Collider>(out var col))
        {
            var ext = col.bounds.extents;
            halfX = ext.x; halfZ = ext.z;
        }
        else { halfX = halfZ = 0.5f; }

        RecalcClamp();
    }

    void Update()
    {
        // Sadece input oku
        float jh = joystick ? joystick.Horizontal : 0f;
        float jv = joystick ? joystick.Vertical   : 0f;

        float kh = (Mathf.Abs(jh) > 0.001f) ? 0f : Input.GetAxisRaw("Horizontal");
        float kv = (Mathf.Abs(jv) > 0.001f) ? 0f : Input.GetAxisRaw("Vertical");

        float h = Mathf.Abs(jh) > 0.001f ? jh : kh;
        float v = Mathf.Abs(jv) > 0.001f ? jv : kv;

        inputDir = new Vector3(h, 0f, v);

        // Deadzone: js çok az oynatıldığında karakterin hareketini sağlar istenmeyen hareketleri eneller
        if (inputDir.magnitude < DEADZONE) inputDir = Vector3.zero;
        else if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        if (inputDir.sqrMagnitude > 0.0001f)
            rb.MoveRotation(Quaternion.LookRotation(inputDir, Vector3.up));
        else
            rb.angularVelocity = Vector3.zero; 
    }

    void FixedUpdate()
    {
        if (inputDir == Vector3.zero)
        {
            // Input yoksa hız sıfırla
            rb.velocity = Vector3.zero;
            return;
        }

        // Bir fizik adımında gitmek istediğimiz mesafe
        Vector3 delta = inputDir * (moveSpeed * Time.fixedDeltaTime);
        Vector3 dir = delta.normalized;
        float dist = delta.magnitude;

        if (rb.SweepTest(dir, out RaycastHit hit, dist, QueryTriggerInteraction.Ignore))
            dist = Mathf.Max(0f, hit.distance - skin);

        Vector3 next = rb.position + dir * dist;

        if (playArea)
        {
            next.x = Mathf.Clamp(next.x, clampX.x, clampX.y);
            next.z = Mathf.Clamp(next.z, clampZ.x, clampZ.y);
        }

        rb.MovePosition(next);
    }

    void RecalcClamp()
    {
        if (!playArea) return;
        Bounds g = playArea.bounds;
        clampX = new Vector2(g.min.x + halfX, g.max.x - halfX);
        clampZ = new Vector2(g.min.z + halfZ, g.max.z - halfZ);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying) return;
        if (playArea != null && TryGetComponent<Collider>(out var col))
        {
            var ext = col.bounds.extents;
            halfX = ext.x; halfZ = ext.z;
            RecalcClamp();
        }
    }
#endif
}
