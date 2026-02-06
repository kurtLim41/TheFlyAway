using UnityEngine;

public class JumpCommand : IPlayerCommand
{
    private PlayerMovement2D _player;
    
    //signals the jump press (concrete command)
    public JumpCommand(PlayerMovement2D player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.JumpPressed();
    }
}