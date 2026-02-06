using UnityEngine;
using TMPro;

public class RunSummaryUI : MonoBehaviour
{
    [System.Serializable]
    private struct LevelRow
    {
        [Tooltip("Level index that this row represents (e.g. 1, 2, 3, ...). " +
                 "Match this to your LevelTimer.levelIndex value.")]
        public int levelIndex;

        public TextMeshProUGUI currentTimeLabel;
        public TextMeshProUGUI bestTimeLabel;
        public GameObject rowRoot;   // whole row object to hide if not played
    }

    [Header("Rows for each possible level")]
    [SerializeField] private LevelRow[] rows;

    private void OnEnable()
    {
        ShowSummary();
    }

    private void ShowSummary()
    {
        foreach (var row in rows)
        {
            bool hasTime = RunStats.TryGetLevelTime(row.levelIndex, out float currentTime);

            if (row.rowRoot != null)
                row.rowRoot.SetActive(hasTime);   // hide rows for levels not played this run

            if (!hasTime)
                continue;

            //Current time:
            float time = Mathf.Clamp(currentTime, 0f, 9999f);

            if (row.currentTimeLabel != null)
                row.currentTimeLabel.text = $"Time: {time:0.00}s";

            // Best time from PlayerPrefs:
            string key = $"Level_{row.levelIndex}_BestTime";
            float best = PlayerPrefs.GetFloat(key, time);
            best = Mathf.Clamp(best, 0f, 9999f);

            if (row.bestTimeLabel != null)
                row.bestTimeLabel.text = $"Best: {best:0.00}s";

            Debug.Log($"[RunSummaryUI] Level {row.levelIndex}: time={time}, best={best}");
        }
    }
}