using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseScreen()
    {
        gameObject.SetActive(true);
    }

    public void HidePauseScreen()
    {
        gameObject.SetActive(false);
    }
}
