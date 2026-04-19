using UnityEngine;
using UnityEngine.InputSystem;

// invoker class
public class InputHandler : MonoBehaviour
{
    [Header("Receiver")]
    [SerializeField] private PlayerMovement2D player;

    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;

    private IPlayerCommand moveLeftCommand;
    private IPlayerCommand moveRightCommand;
    private IPlayerCommand stopMoveCommand;
    private IPlayerCommand jumpCommand;

    private bool isPaused;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (!player)
            player = GetComponent<PlayerMovement2D>();

        playerInput = player.GetComponent<PlayerInput>();
        
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Build commands once (Command Pattern stays intact)
        moveLeftCommand  = new MoveCommand(player, -1f);
        moveRightCommand = new MoveCommand(player,  1f);
        stopMoveCommand  = new MoveCommand(player,  0f);
        jumpCommand      = new JumpCommand(player);
    }

    private void OnEnable()
    {
        PauseManager.OnPauseChanged += HandlePause;
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= HandlePause;
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        if (isPaused) return;

        HandleMovement();
        HandleJump();
    }

    private void HandlePause(bool isPaused)
    {
        this.isPaused = isPaused;
    }
    
    private void HandleMovement()
    {
        float move = moveAction.ReadValue<Vector2>().x;

        if (move < -0.1f)
            moveLeftCommand.Execute();
        else if (move > 0.1f)
            moveRightCommand.Execute();
        else
            stopMoveCommand.Execute();
    }

    private void HandleJump()
    {
        if (jumpAction.triggered)
            jumpCommand.Execute();

        player.SetJumpHeld(jumpAction.IsPressed());
    }
}
