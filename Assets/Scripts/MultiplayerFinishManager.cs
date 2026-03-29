using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerFinishManager : MonoBehaviour
{
    public static MultiplayerFinishManager I { get; private set; }

    [Header("References")]
    [SerializeField] private LevelTimer levelTimer;

    [SerializeField] private PlayerFinishUI player1UI;
    [SerializeField] private PlayerFinishUI player2UI;

    private bool p1Done;
    private bool p2Done;
    private bool _advancing;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    private void Start()
    {
        // Auto-find timer if not assigned (optional but helpful)
        if (levelTimer == null)
            levelTimer = FindFirstObjectByType<LevelTimer>();

        player1UI?.Hide();
        player2UI?.Hide();
    }

    public void PlayerReachedGoal(int playerId, Transform playerRoot)
    {
        if (_advancing) return;

        float t = levelTimer != null ? levelTimer.ElapsedTime : Time.timeSinceLevelLoad;

        if (playerId == 1 && !p1Done)
        {
            p1Done = true;
            player1UI?.Show(t);
            FreezePlayer(playerRoot);
            Destroy(playerRoot.gameObject);
        }
        else if (playerId == 2 && !p2Done)
        {
            p2Done = true;
            player2UI?.Show(t);
            FreezePlayer(playerRoot);
            Destroy(playerRoot.gameObject);
        }

        if (p1Done && p2Done && !_advancing)
        {
            _advancing = true;

            // Record final time once (second finisher time)
            if (levelTimer != null)
                levelTimer.FinishLevel();

            // Advance playlist
            if (GameFlow.I != null)
                GameFlow.I.LevelCompleted();
        }
    }

    private void FreezePlayer(Transform playerRoot)
    {
        if (!playerRoot) return;
        
        var rb = playerRoot.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;
        
        var input = playerRoot.GetComponent<InputHandler>();
        if (input) input.enabled = false;
    }
}
