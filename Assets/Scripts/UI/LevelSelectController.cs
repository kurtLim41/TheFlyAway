using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // if you use TextMeshPro

public class LevelSelectController : MonoBehaviour
{
    [Header("Available level scene names (must be in Build Settings)")]
    [SerializeField] private List<string> availableLevelScenes = new List<string>();

    [Header("UI")]
    [SerializeField] private Button playGameButton;
    [SerializeField] private TextMeshProUGUI selectedCountLabel;

    private HashSet<string> selected = new HashSet<string>();
    private int maxSelections;

    void Start()
    {
        maxSelections = GameFlow.I?.MaxSelections ?? 3;
        UpdateUI();
        // You’ll wire each map button’s OnClick to call ToggleLevel(sceneName)
    }

    public void ToggleLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        if (selected.Contains(sceneName))
        {
            selected.Remove(sceneName);
            Debug.Log("ToggleLevel called for: " + sceneName);
        }
        else
        {
            if (selected.Count < maxSelections)
                selected.Add(sceneName);
            else
                Debug.Log("Already at max selections.");
        }

        UpdateUI();
    }

    public void PlayGame()
    {
        if (selected.Count != maxSelections)
        {
            Debug.Log("Select exactly " + maxSelections);
            return;
        }

        GameFlow.I.SetSelection(selected);
        GameFlow.I.StartRun();
    }

    public void BackToTitle()
    {
        GameFlow.I.AbortToTitle();
    }

    private void UpdateUI()
    {
        selectedCountLabel.text = $"Selected {selected.Count}/{maxSelections}";
        playGameButton.interactable = (selected.Count == maxSelections);
    }
}