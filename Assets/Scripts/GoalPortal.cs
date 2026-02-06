using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GoalPortal : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f;
    public UnityEvent onGoalReached;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        onGoalReached?.Invoke();
        StartCoroutine(DoComplete());
    }

    private IEnumerator DoComplete()
    {
        yield return new WaitForSeconds(delay);

        if (GameFlow.I != null)
        {
            GameFlow.I.LevelCompleted();
        }
        else
        {
            // Fallback: if GameFlow missing, just reload current
            var s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(s);
        }
    }
}