using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    public void OnStartGamePressed()
    {
        if (GameFlow.I != null)
        {
            GameFlow.I.BackToSelect();
        }
        else
        {
            Debug.LogError("GameFlow singleton not found!");
        }
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}