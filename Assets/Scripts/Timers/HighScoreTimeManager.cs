using UnityEngine;

//observer 
public class HighScoreTimeManager : MonoBehaviour
{
    private void OnEnable()
    {
        LevelEvents.OnLevelCompleted += HandleLevelCompleted;
    }

    private void OnDisable()
    {
        LevelEvents.OnLevelCompleted -= HandleLevelCompleted;
    }

    private void HandleLevelCompleted(int levelIndex, float time)
    {
        string key = GetBestTimeKey(levelIndex);

        float bestTime = PlayerPrefs.GetFloat(key, float.MaxValue);

        // Save if this run is better 
        if (time < bestTime)
        {
            PlayerPrefs.SetFloat(key, time);
            PlayerPrefs.Save();
        }
    }

    private string GetBestTimeKey(int levelIndex)
    {
        return $"Level_{levelIndex}_BestTime";
    }
}