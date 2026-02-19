using UnityEngine;

// invoker class
public class InputHandler : MonoBehaviour
{
    public enum ControlScheme
    {
        WASD,
        Arrows
    }

    [Header("Control Scheme")]
    [SerializeField] private ControlScheme scheme = ControlScheme.WASD;

    [Header("Receiver")]
    [SerializeField] private PlayerMovement2D player;

    private IPlayerCommand moveLeftCommand;
    private IPlayerCommand moveRightCommand;
    private IPlayerCommand stopMoveCommand;
    private IPlayerCommand jumpCommand;

    private bool isPaused;

    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode jumpKey;

    private void Awake()
    {
        if (!player)
            player = GetComponent<PlayerMovement2D>();

        // Build commands once (Command Pattern stays intact)
        moveLeftCommand  = new MoveCommand(player, -1f);
        moveRightCommand = new MoveCommand(player,  1f);
        stopMoveCommand  = new MoveCommand(player,  0f);
        jumpCommand      = new JumpCommand(player);

        // Assign keys based on scheme
        ApplyScheme();
    }

    private void ApplyScheme()
    {
        if (scheme == ControlScheme.WASD)
        {
            leftKey  = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey  = KeyCode.W;
        }
        else // Arrows
        {
            leftKey  = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey  = KeyCode.UpArrow;
        }
    }

    private void OnEnable()
    {
        PauseManager.OnPauseChanged += HandlePause;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= HandlePause;
    }

    private void Update()
    {
        if (isPaused) return;

        // Horizontal movement
        if (Input.GetKey(leftKey))
            moveLeftCommand.Execute();
        else if (Input.GetKey(rightKey))
            moveRightCommand.Execute();
        else
            stopMoveCommand.Execute();

        // Jump press 
        if (Input.GetKeyDown(jumpKey))
            jumpCommand.Execute();

        // Jump held
        bool jumpHeld = Input.GetKey(jumpKey);
        player.SetJumpHeld(jumpHeld);
    }

    private void HandlePause(bool isPaused)
    {
        this.isPaused = isPaused;
    }
}
