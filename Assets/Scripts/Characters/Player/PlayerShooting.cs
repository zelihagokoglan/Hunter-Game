using UnityEngine;
using System.Linq;

public class PlayerShooter : MonoBehaviour
{
    public Transform firePoint; // merminin çıkacağı nokta
    public Projectile projectilePrefab; // mermi prefabı
    public float fireInterval = 3f;         // hangi sıklıkla mermi gönderileceği
    public float maxAcquireRange = 100f;    // düşmanı arama yarıçapı
    public float retargetInterval = 0.25f; // hangi sıklıkla düşmanı arayacağı 

    Transform currentTarget; // şu an hedeflenen düşman
    float retargetTimer; // zamanlayıcılar
    float fireTimer; // başlangıç mermi atışı içinn değişken sayaç

    void Start()
    {
        fireTimer = fireInterval; // başlangıçta hemen ateşlemek için sayaç dolu başlar
    }

    void Update()
    {
        retargetTimer += Time.deltaTime; // Hedefi yeniden aramak için geçen süreyi sayar.
        fireTimer     += Time.deltaTime; // Mermiyi yeniden ateşlemek için geçen süreyi sayar.

        if (currentTarget == null || retargetTimer >= retargetInterval) // şu anda radius içinde bir hedef yoksa veya hedef arama sıklığı kaç saniyede aranacağı sıklığını geçtiyse
        {
            currentTarget = FindNearestEnemy(); // en yakın hedefi ara
            retargetTimer = 0f; // hedef arama sıklığını sıfırla
        }
        if (currentTarget == null) return; // eğer hala hedef yoksa çık update kalanı çalışmaz

        Vector3 aimPoint = GetTargetPoint(currentTarget); // hedeflenecek noktayı şu anki hedef olarak belirliyor
        Vector3 toTarget = aimPoint - transform.position; // şu anki konum ve hedef arasındaki vektör
        toTarget.y = 0f; // y ekseni dahil edilmiyor vektöre
        
        if (toTarget.sqrMagnitude > 0.0001f) // hedefle arasındaki mesafe çok küçük değilse karakterin baktığı yönü hedefe çevir
            transform.forward = toTarget.normalized;

        if (fireTimer >= fireInterval && firePoint && projectilePrefab) // Son atıştan bu yana geçen süre, ateşleme aralığını geçtiyse. firepoint ve projectile atanmış mı bakar
        {
            fireTimer = 0f; // sayaç sıfırlanır
            Vector3 dir = (aimPoint - firePoint.position).normalized; // dir: merminin başlangıç yönü ve ateş ucu ile hedef arasındaki normalize edilmiş vektör.
            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.FireAt(currentTarget, dir, Vector3.zero, "Player");

            var killer = proj.GetComponent<DestroyOnObstacleHit>();
            if (killer) killer.Init("Player");

            proj.FireAt(currentTarget, dir, Vector3.zero, "Player");   
        }
    }
    Transform FindNearestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        Transform best = null; // şua ana kadar bulunan en yakın düşman
        float bestDistSq = float.MaxValue; // şu ana kadarki en kğçğk mesafe MaxValue verilir ilk karşılaştırma yaparken her düşmanın daha yakın görünmesini sağlamak amacıyla
        float rangeSq = maxAcquireRange * maxAcquireRange;
        Vector3 myPos = transform.position; // playerın pozisyonu

        foreach (var e in enemies)
        {
            float distSq = (e.transform.position - myPos).sqrMagnitude;
            if (distSq < bestDistSq && distSq <= rangeSq)
            {
                bestDistSq = distSq;
                best = e.transform;
            }
        }
        return best;
    }

    Vector3 GetTargetPoint(Transform t)
    {
        if (t.TryGetComponent<Collider>(out var col))
            return col.bounds.center;

        return t.position;
    }
}
