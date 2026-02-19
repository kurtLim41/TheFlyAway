using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [Tooltip("Set this to 1, 2, 3 depending on which level this is.")]
    public int levelIndex = 1;

    [SerializeField] private Text timerText;

    private float _elapsedTime;
    private bool _isRunning = true;
    private bool _submitted;

    public float ElapsedTime => _elapsedTime;

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(_elapsedTime);
        }
    }

    //called when player finishes level
    public void FinishLevel()
    {
        if (_submitted) return;

        _isRunning = false;
        _submitted = true;

        // Store the runs time for the level
        RunStats.RecordLevelTime(levelIndex, _elapsedTime);

        // Observer: notify any listeners (HighScoreTimeManager)
        LevelEvents.RaiseLevelCompleted(levelIndex, _elapsedTime);
    }
    private void UpdateTimerDisplay(float time)
    {
        timerText.text = TimeToString(time);
    }
    
    private string TimeToString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        string text = $"{minutes:0}:{seconds:00}";
        return text;
    }

}