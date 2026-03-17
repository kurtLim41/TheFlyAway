using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
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
        panel.SetActive(false);
    }

    public void QuitGame()
    {
        GameFlow.I.QuitGame();
    }

    public void ReturnToMenu()
    {
        GameFlow.I.AbortToTitle();
    }
    
    private void ToggleScreen(bool isPaused)
    {
        panel.SetActive(isPaused);
    }
}
