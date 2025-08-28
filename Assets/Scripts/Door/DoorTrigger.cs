using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] DoorController door;
    [SerializeField] string playerTag = "Player";
    [SerializeField] string nextSceneName = ""; 
    [SerializeField] string fallbackScene = "";
    [SerializeField] DoorLightController lightController;

    void OnTriggerEnter(Collider other)
    {
        bool doorOpen = door && door.IsOpen;
        Debug.Log($"[DoorTrigger] Trigger: {other.name}, doorOpen={doorOpen}");

        if (!doorOpen) return;
        if (!other.CompareTag(playerTag)) return;

        lightController?.TurnOffLight();

        int coins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", coins + 100);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"[DoorTrigger] Load by name: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"[DoorTrigger] Load by index: {current} -> {next}");
            SceneManager.LoadScene(next);
        }
        else
        {
            Debug.Log("[DoorTrigger] Last level reached.");
            if (!string.IsNullOrEmpty(fallbackScene))
                SceneManager.LoadScene(fallbackScene);
            else
                SceneManager.LoadScene(0);
        }
    }
}