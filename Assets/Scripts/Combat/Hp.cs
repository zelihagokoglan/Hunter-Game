using UnityEngine;

public class Hp : MonoBehaviour
{
    public int maxHp = 200;
    public int currentHp;

    [Header("UI (opsiyonel ama önerilir)")]
    [SerializeField] GameOverUI gameOverUI; // Inspector’dan atayabilirsin

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
            // Düşman ölünce
            SendMessage("TriggerExplosion", SendMessageOptions.DontRequireReceiver);
            StaticEvents.RaiseEnemyDied();   // Sayaç azalsın
            Destroy(gameObject);
        }
        else if (CompareTag("Player"))
        {
            // Player ölünce her zaman Game Over
            if (gameOverUI) gameOverUI.Show();
            else
            {
                // Referans yoksa bulmaya çalış (daha pahalı ama güvenlik)
                var go = FindObjectOfType<GameOverUI>();
                if (go) go.Show();
            }

            gameObject.SetActive(false);
        }
    }
}