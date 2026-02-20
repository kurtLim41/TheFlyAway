using TMPro;
using UnityEngine;

public class PlayerFinishUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timeText;

    public void Hide()
    {
        if (panel) panel.SetActive(false);
    }

    public void Show(float seconds)
    {
        if (panel) panel.SetActive(true);
        if (timeText) timeText.text = $"Completed! Time: {seconds:0}:{(int)(seconds % 60):00}";
        // If you want decimals instead:
        // timeText.text = $"Completed! Time: {seconds:0.00}s";
    }
}