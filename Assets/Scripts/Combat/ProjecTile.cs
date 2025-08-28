using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int baseDamage = 20;  // player için basit hasar bunu upgrade ile çarpacağım

    Transform _target;
    Vector3 _dir;
    Quaternion _visualOffset = Quaternion.identity;
    string _shooterTag;

    public void FireAt(Transform target, Vector3 startDirection, Vector3 rotationOffsetEuler, string shooterTag)
    {
        _target = target;
        _dir = startDirection.normalized;
        _visualOffset = Quaternion.Euler(rotationOffsetEuler);
        _shooterTag = shooterTag;
    }

    void Update()
    {
        transform.position += _dir * (speed * Time.deltaTime);

        if (_dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(_dir) * _visualOffset;
    }

    Vector3 GetPoint(Transform t)
    {
        if (t && t.TryGetComponent<Collider>(out var col)) return col.bounds.center;
        return t ? t.position : transform.position + _dir;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_shooterTag)) return;

        if (_shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Hp hp = other.GetComponent<Hp>();
            if (hp != null)
            {
                int dmgLevel = PlayerPrefs.GetInt("Damage", 1);
                float dmgMult = 1f + 0.3f * (dmgLevel - 1); // seviye başı %30 artış
                int finalDamage = Mathf.RoundToInt(baseDamage * dmgMult);
                hp.TakeDamage(finalDamage);
            }
            Destroy(gameObject);
        }
        else if (_shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            Hp hp = other.GetComponent<Hp>();
            if (hp != null)
            {
                hp.TakeDamage(baseDamage); // düşman mermileri upgrade almaz
            }
            Destroy(gameObject);
        }
    }
}