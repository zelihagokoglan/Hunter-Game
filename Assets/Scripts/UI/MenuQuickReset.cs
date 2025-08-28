using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuQuickReset : MonoBehaviour
{
    // OnClick te çağır
    public void ResetToDefault()
    {
        PlayerPrefs.SetInt("Coins", 200);
        PlayerPrefs.SetInt("UP_Speed_LEVEL", 1);
        PlayerPrefs.SetInt("UP_Damage_LEVEL", 1);
        PlayerPrefs.SetInt("UP_Health_LEVEL", 1);
        PlayerPrefs.Save();

        var mm = FindObjectOfType<MainMenuUI>();
        if (mm != null)
        {
            mm.RefreshMenu();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}