using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [Header("Bağlantılar")]
    [SerializeField] GameObject panel;               // WinPanel (başta kapalı)
    [SerializeField] string menuSceneName = "Menu";  // Menü sahnesinin adı
    [SerializeField] bool pauseOnShow = true;        // Açılınca oyunu dondur

    void Awake()
    {
        if (panel) panel.SetActive(false);
    }

    public void Show()
    {
        if (panel) panel.SetActive(true);
        if (pauseOnShow) Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (panel) panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Buton OnClick -> Menüyeye Dön
    public void OnGoToMenu()
    {
        Time.timeScale = 1f; // Sahne değişmeden önce düzelt
        SceneManager.LoadScene(menuSceneName);
    }

    // Editor’de hızlı deneme için
    [ContextMenu("Test Show")]
    void __TestShow() => Show();
}