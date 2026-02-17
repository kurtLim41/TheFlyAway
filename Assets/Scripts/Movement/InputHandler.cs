using UnityEngine;


//invoker class 
public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement2D player;

    private IPlayerCommand moveLeftCommand;
    private IPlayerCommand moveRightCommand;
    private IPlayerCommand stopMoveCommand;
    private IPlayerCommand jumpCommand;

    private bool isPaused;
    
    private void Awake()
    {
        if (!player)
            player = GetComponent<PlayerMovement2D>();

        moveLeftCommand  = new MoveCommand(player, -1f);
        moveRightCommand = new MoveCommand(player,  1f);
        stopMoveCommand  = new MoveCommand(player,  0f);

        jumpCommand      = new JumpCommand(player);
    }

    void OnEnable()
    {
        PauseManager.OnPauseChanged += HandlePause;
    }

    void OnDisable()
    {
        PauseManager.OnPauseChanged -= HandlePause;
    }
    
    //reads WAD/Space input and executes command 
    private void Update()
    {
        // let paused state override input settings for movement
        if (isPaused)
        {
            return;
        }
        
        // Horizontal movement 
        if (Input.GetKey(KeyCode.A))
            moveLeftCommand.Execute();
        else if (Input.GetKey(KeyCode.D))
            moveRightCommand.Execute();
        else
            stopMoveCommand.Execute();

        // Jump press
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            jumpCommand.Execute();

        // Jumpt held state 
        bool jumpHeld = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        player.SetJumpHeld(jumpHeld);
    }

    private void HandlePause(bool isPaused)
    {
        this.isPaused = isPaused;
    }
}