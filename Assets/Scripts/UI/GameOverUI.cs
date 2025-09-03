using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // TMP değilse Button için

public class GameOverUI : MonoBehaviour
{
    [Header("Bağlantılar")]
    [SerializeField] GameObject panel;        // GameOverPanel (içinde buton var)
    [SerializeField] string menuSceneName = "Menu";
    [SerializeField] bool pauseOnShow = true;

    void Awake()
    {
        if (panel) panel.SetActive(false);    // başta kapalı
    }

    public void Show()
    {
        if (panel) panel.SetActive(true);
        if (pauseOnShow) Time.timeScale = 0f;  // oyunu dondur
    }

    public void Hide()
    {
        if (panel) panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Butonun OnClick'ine bağlayacağın fonksiyon
    public void OnRestartToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    // İstersen aynı panelden "Aynı Leveli Yeniden Başlat" da ekleyebilirsin:
    public void OnRestartSameLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
