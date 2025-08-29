using UnityEngine;
using System.Reflection;

[DisallowMultipleComponent]
public class PlayerUpgradeApplier : MonoBehaviour
{
    public MonoBehaviour movementScript;
    public string speedFieldOrPropertyName = "moveSpeed";
    public float speedPerLevel = 0.2f;
    public int healthPerLevel = 20;

    void Start()
    {
        ApplySpeedUpgrade(); // hız yükseltir
        ApplyHealthUpgrade(); // can hp yükseltir
    }

    void ApplySpeedUpgrade() // Oyuncunun hız seviyesini PlayerPrefs’ten alır.
                            // Seviye arttıkça hız çarpanı hesaplanır:
    {
        int speedLevel = PlayerPrefs.GetInt("Speed", 1);
        float speedMult = 1f + speedPerLevel * (speedLevel - 1);

        if (movementScript == null || string.IsNullOrEmpty(speedFieldOrPropertyName))
            return;

        var t = movementScript.GetType();

        var field = t.GetField(speedFieldOrPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(float))
        {
            float current = (float)field.GetValue(movementScript);
            field.SetValue(movementScript, current * speedMult);
            return;
        }
        var prop = t.GetProperty(speedFieldOrPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null && prop.PropertyType == typeof(float) && prop.CanRead && prop.CanWrite)
        {
            float current = (float)prop.GetValue(movementScript, null);
            prop.SetValue(movementScript, current * speedMult, null);
            return;
        }

        Debug.LogWarning($"[PlayerUpgradeApplier] '{t.Name}' üzerinde '{speedFieldOrPropertyName}' float field/property bulunamadı. Hız uygulanamadı.");
    }

    void ApplyHealthUpgrade() // Oyuncunun health seviyesini PlayerPrefs’ten alır.
                                // Ekstra can miktarını hesaplar:
    {
        int healthLevel = PlayerPrefs.GetInt("Health", 1);
        int extraHp = healthPerLevel * (healthLevel - 1);

        if (TryGetComponent<Hp>(out var hp))
        {
            hp.maxHp += extraHp;
            hp.currentHp = hp.maxHp; // full can başlat
        }
        else
        {
            Debug.LogWarning("[PlayerUpgradeApplier] Bu GameObject'te Hp bileşeni yok. Health uygulanamadı.");
        }
    }
}
