using UnityEngine;
using UnityEngine.UI;

public class PauseButtonIcon : MonoBehaviour
{
    [Header("İkonlar")]
    [SerializeField] private Sprite pauseSprite;  
    [SerializeField] private Sprite playSprite;   
    [Header("Hedef Image")]
    [SerializeField] private Image targetImage;   

    void OnEnable()
    {
        GamePauseManager.OnPauseChanged += HandlePauseChanged;

        if (GamePauseManager.Instance != null)
            HandlePauseChanged(GamePauseManager.Instance.IsPaused);
        else
            HandlePauseChanged(false);
    }

    void OnDisable()
    {
        GamePauseManager.OnPauseChanged -= HandlePauseChanged;
    }

    void HandlePauseChanged(bool isPaused)
    {
        if (targetImage != null)
            targetImage.sprite = isPaused ? playSprite : pauseSprite;
    }
}