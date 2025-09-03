using UnityEngine;

public class WinListener : MonoBehaviour
{
    [Header("Sadece son levelde TRUE")]
    [SerializeField] bool isFinalLevel = true;

    [Header("UI")]
    [SerializeField] WinUI winUI;

    void OnEnable()  { StaticEvents.AllEnemiesDead += OnAllEnemiesDead; }
    void OnDisable() { StaticEvents.AllEnemiesDead -= OnAllEnemiesDead; }

    void OnAllEnemiesDead()
    {
        if (!isFinalLevel) return;
        if (winUI) winUI.Show();
        else Debug.LogWarning("[WinListener] winUI referansı eksik!");
    }
}