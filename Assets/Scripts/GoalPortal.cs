using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalPortal : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f;
    public UnityEvent onGoalReached;

    private readonly HashSet<int> triggeredPlayers = new HashSet<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var identity = other.GetComponentInParent<PlayerIdentity>();
        if (identity == null)
        {
            Debug.LogWarning("GoalPortal: Player hit portal but no PlayerIdentity found in parents.");
            return;
        }

        if (triggeredPlayers.Contains(identity.playerId)) return;
        triggeredPlayers.Add(identity.playerId);
        
        onGoalReached?.Invoke();

     
        Transform playerRoot = other.GetComponentInParent<PlayerMovement2D>()?.transform;
        if (playerRoot == null)
            playerRoot = other.GetComponentInParent<InputHandler>()?.transform;
        if (playerRoot == null)
            playerRoot = other.transform; // last resort

        // Multiplayer path 
        if (MultiplayerFinishManager.I != null)
        {
            MultiplayerFinishManager.I.PlayerReachedGoal(identity.playerId, playerRoot);
            return;
        }

        // Single-player fallback 
        StartCoroutine(DoCompleteSinglePlayer());
    }

    private IEnumerator DoCompleteSinglePlayer()
    {
        yield return new WaitForSeconds(delay);

        if (GameFlow.I != null)
        {
            GameFlow.I.LevelCompleted();
        }
        else
        {
            var s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(s);
        }
    }
}