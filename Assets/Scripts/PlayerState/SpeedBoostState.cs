using UnityEngine;

public class SpeedBoostState : IPlayerState
{
    private float _duration;
    private float _timer;
    private float _speedMultiplier;
    

    public SpeedBoostState(float duration, float speedMultiplier = 1.5f, int extraJumps = 0)
    {
        _duration       = duration;
        _speedMultiplier = speedMultiplier;
    }

    public void Enter(PlayerStateMachine ctx)
    {
        _timer = _duration;
        ctx.Player.SetMoveSpeedMultiplier(_speedMultiplier);
        Debug.Log("[PlayerState] Enter SpeedBoostState");
    }


    public void Exit(PlayerStateMachine ctx)
    {
        // Return to base stats
        ctx.Player.ResetStatsToBase();
    }

    public void Update(PlayerStateMachine ctx, float deltaTime)
    {
        _timer -= deltaTime;
        if (_timer <= 0f)
        {
            // Gooes back to normal when boost expires
            ctx.ChangeState(new NormalState());
        }
    }
}