using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [Tooltip("Set this to 1, 2, 3 depending on which level this is.")]
    public int levelIndex = 1;

    private float _elapsedTime;
    private bool _isRunning = true;
    private bool _submitted;

    public float ElapsedTime => _elapsedTime;

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
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

}