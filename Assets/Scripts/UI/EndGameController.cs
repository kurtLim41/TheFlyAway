using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public void PlayAgain()
    {
        GameFlow.I.BackToSelect();
    }

    public void BackToTitle()
    {
        GameFlow.I.AbortToTitle();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}