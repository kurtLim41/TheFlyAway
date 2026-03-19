using UnityEngine;
using UnityEngine.UI;

public class MapToggleController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mapPanel; 
    
    public Button closeButton; 
    
    private bool isMapOpen = false;

    void Start()
    {
        if (mapPanel != null)
        {
            mapPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("MapToggleController: You forgot to drag the Map Panel game object into the slot!");
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnMAPPButtonClick); // Uses the same toggle logic
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMAPPButtonClick();
        }
    }

    public void OnMAPPButtonClick()
    {
        if (mapPanel == null) return;

        isMapOpen = !isMapOpen;

        mapPanel.SetActive(isMapOpen);

        Time.timeScale = isMapOpen ? 0f : 1f;

        if (isMapOpen)
        {
            Debug.Log("<color=pink>MAPP</color> Opened - Game Paused.");
        }
        else
        {
            Debug.Log("<color=pink>MAPP</color> Closed - Game Resumed.");
        }
    }
}