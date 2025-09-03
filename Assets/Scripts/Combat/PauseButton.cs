using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void OnClickToggle()
    {
        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.TogglePause();
    }
}