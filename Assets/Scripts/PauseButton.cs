using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private PauseScreenUI pauseUI;

    public void Pause()
    {
        PauseManager.SetPaused(true);
        Debug.Log("Paused");
    }
    
    public void Resume()
    {
        PauseManager.SetPaused(false);
    }

    public void OpenMap()
    {
        PauseManager.SetPaused(true);
        
        if (pauseUI != null)
        {
            pauseUI.OpenMap();
        }
    }
}