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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);
    }
    
    private void ToggleScreen(bool isPaused)
    {
        panel.SetActive(isPaused);
    }
}
