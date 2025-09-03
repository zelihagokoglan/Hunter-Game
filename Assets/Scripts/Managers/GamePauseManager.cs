using System;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }
    public static event Action<bool> OnPauseChanged;

    public bool IsPaused { get; private set; }
    float _defaultFixedDelta;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _defaultFixedDelta = Time.fixedDeltaTime;
        ApplyPause(false, true);
    }

    public void TogglePause() => ApplyPause(!IsPaused);
    public void Pause()       => ApplyPause(true);
    public void Resume()      => ApplyPause(false);

    void ApplyPause(bool pause, bool silent = false)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        Time.fixedDeltaTime = _defaultFixedDelta * Time.timeScale;

        if (!silent) OnPauseChanged?.Invoke(IsPaused);
    }
}