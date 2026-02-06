using UnityEngine;

public class MoveCommand : IPlayerCommand
{
    private PlayerMovement2D _player;
    private float _direction;

    //changes horixontal movement of character (concrete command)
    public MoveCommand(PlayerMovement2D player, float direction)
    {
        _player = player;
        _direction = direction;
    }

    public void Execute()
    {
        _player.SetMoveInput(_direction);
    }
}