using UnityEngine;

public class Hp : MonoBehaviour
{
    public int maxHp = 200;
    public int currentHp;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Max(currentHp, 0);
        
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (CompareTag("Enemy"))
        {
            SendMessage("TriggerExplosion", SendMessageOptions.DontRequireReceiver);

            StaticEvents.RaiseEnemyDied();
        }

        Destroy(gameObject);
    }

}