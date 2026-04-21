using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    public GameObject player1JoinedText;
    public GameObject player2JoinedText;
    
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
        GameFlow.I.QuitGame();
    }

    public void ShowText(int playerId)
    {
        if (playerId == 1)
        {
            StartCoroutine(ShowTextCoroutine(player1JoinedText));
        }
        else if (playerId == 2)
        {
            StartCoroutine(ShowTextCoroutine(player2JoinedText));
        }
    }
    
    private IEnumerator ShowTextCoroutine(GameObject text)
    {
        text.SetActive(true);
        
        Text uiText = text.GetComponent<Text>();
        Color originalColor = uiText.color;

        // Make sure it's fully visible at start
        uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(1f);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / duration);

            uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        text.SetActive(false);
        
    }
}