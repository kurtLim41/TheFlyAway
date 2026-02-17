using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void Pause()
    {
        PauseManager.SetPaused(true);
        Debug.Log("Paused");
    }
    
    public void Resume()
    {
        PauseManager.SetPaused(false);
    }
}
