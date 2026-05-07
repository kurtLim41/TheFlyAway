using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel; // Renamed from 'panel' for clarity
    [SerializeField] private GameObject mapPanel;
    
    void OnEnable()
    {
        PauseManager.OnPauseChanged += ToggleScreen;
    }

    void OnDisable()
    {
        PauseManager.OnPauseChanged -= ToggleScreen;
    }
    
    void Start()
    {
        mainPanel.SetActive(false);
        mapPanel.SetActive(false);
    }

    public void QuitGame()
    {
        GameFlow.I.QuitGame();
    }

    public void ReturnToMenu()
    {
        GameFlow.I.AbortToTitle();
    }

    public void OpenMap()
    {
        mainPanel.SetActive(false);
        mapPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        mapPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    
    private void ToggleScreen(bool isPaused)
    {
        mainPanel.SetActive(isPaused);
        
        if (!isPaused)
        {
            mapPanel.SetActive(false);
        }
    }
}