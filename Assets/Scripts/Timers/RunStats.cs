using System.Collections.Generic;

public static class RunStats
{
    // Stores the time for each level played in a run 
    // Key: levelIndex, Value: time in seconds
    private static Dictionary<int, float> _levelTimes = new Dictionary<int, float>();

    public static void RecordLevelTime(int levelIndex, float time)
    {
        _levelTimes[levelIndex] = time;
    }

    public static bool TryGetLevelTime(int levelIndex, out float time)
    {
        return _levelTimes.TryGetValue(levelIndex, out time);
    }

    public static void Clear()
    {
        _levelTimes.Clear();
    }
}