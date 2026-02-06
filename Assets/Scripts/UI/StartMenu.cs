using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject startPanel;
    
    public GameObject gameplayRoot;
    

    private bool _started;

    private void Awake()
    {
        if (startPanel != null) startPanel.SetActive(true);
        if (gameplayRoot != null) gameplayRoot.SetActive(false);
        _started = false;
    }

    // Hook this to the Button's OnClick
    public void OnStartGameClicked()
    {
        if (_started) return;
        _started = true;

        if (startPanel != null) startPanel.SetActive(false);
        if (gameplayRoot != null) gameplayRoot.SetActive(true);

        // Later: SceneManager.LoadScene("Level1"); or raise a GameStarted event
    }    



}
