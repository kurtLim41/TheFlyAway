using UnityEngine;
using TMPro;

public class LevelResultsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentTimeLabel;
    [SerializeField] private TextMeshProUGUI bestTimeLabel;
    
    // This scene should already be your Game Over UI.
    // No need to enable/disable panels here.

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
        // Set Current Time
        if (currentTimeLabel != null)
            currentTimeLabel.text = $"Time: {time:F2}s";

        // Pull best time from PlayerPrefs
        string key = $"Level_{levelIndex}_BestTime";
        float best = PlayerPrefs.GetFloat(key, time);

        if (bestTimeLabel != null)
            bestTimeLabel.text = $"Best: {best:F2}s";
    }
}