using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameFlow : MonoBehaviour
{
    public static GameFlow I { get; private set; }
    private static GameFlow _instance;

    [Header("Scene Names")]
    [SerializeField] private string titleScene = "TitleScreen";
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string endGameScene = "EndGame";

    [Header("Run Settings")]
    [SerializeField] private int maxSelections = 3;

    private readonly List<string> playlist = new List<string>();
    private int currentIndex = -1;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        
        // Singleton pattern so only one GameFlow exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // --- Public API ---

    public int MaxSelections => maxSelections;
    public IReadOnlyList<string> CurrentPlaylist => playlist;

    public void SetSelection(IEnumerable<string> sceneNames)
    {
        playlist.Clear();
        foreach (var s in sceneNames)
            if (!string.IsNullOrWhiteSpace(s))
                playlist.Add(s);

        // optional dedupe
        // for (int i = playlist.Count - 1; i >= 0; i--)
        // {
        //     if (playlist.FindIndex(j => playlist[j] == playlist[i]) != i)
        //         playlist.RemoveAt(i);
        // }

        if (playlist.Count > maxSelections)
            playlist.RemoveRange(maxSelections, playlist.Count - maxSelections);
    }

    public void StartRun()
    {
        if (playlist.Count == 0)
        {
            Debug.LogWarning("GameFlow: playlist empty — sending to LevelSelect.");
            SceneManager.LoadScene(levelSelectScene);
            return;
        }
        currentIndex = -1;
        LoadNext();
    }

    public void LevelCompleted()
    {
        LoadNext();
    }

    public void AbortToTitle()
    {
        playlist.Clear();
        currentIndex = -1;
        SceneManager.LoadScene(titleScene);
    }

    public void BackToSelect()
    {
        playlist.Clear();
        currentIndex = -1;
        SceneManager.LoadScene(levelSelectScene);
    }

    public int LevelsRemaining()
    {
        return Mathf.Max(0, playlist.Count - (currentIndex + 1));
    }

    // --- Internals ---

    private void LoadNext()
    {
        currentIndex++;
        if (currentIndex < playlist.Count)
        {
            var nextScene = playlist[currentIndex];
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene(endGameScene);
        }
    }
}
