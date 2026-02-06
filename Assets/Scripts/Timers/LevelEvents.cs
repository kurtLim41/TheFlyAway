using System;

public static class LevelEvents
{
    // levelIndex 1,2,3...
    public static event Action<int, float> OnLevelCompleted;

    public static void RaiseLevelCompleted(int levelIndex, float time)
    {
        OnLevelCompleted?.Invoke(levelIndex, time);
    }
}