using System;
using UnityEngine;
using UnityEngine.XR;

public static class PauseManager
{
    public static bool IsPaused { get; private set; } = false;
    public static event Action<bool> OnPauseChanged;

    public static void SetPaused(bool paused)
    {
        if (IsPaused == paused) return; 
        IsPaused = paused;
        OnPauseChanged?.Invoke(paused);
        Debug.Log("Set paused : " + paused);
        HandleTime();
    }

    private static void HandleTime()
    {
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}
