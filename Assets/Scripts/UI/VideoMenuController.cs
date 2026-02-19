using UnityEngine;

public class VideoMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject videoPanel;

    public void OpenVideo()
    {
        mainMenuPanel.SetActive(false);
        videoPanel.SetActive(true);
    }

    public void CloseVideo()
    {
        videoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}