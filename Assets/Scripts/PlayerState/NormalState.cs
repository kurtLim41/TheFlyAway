using UnityEngine;

public class NormalState : IPlayerState
{
    public void Enter(PlayerStateMachine ctx)
    {
        // Reset stats to original values
        ctx.Player.ResetStatsToBase();
        Debug.Log("[PlayerState] Enter NormalState");
    }

    public void Exit(PlayerStateMachine ctx)
    {
        // needs nothing here
    }

    public void Update(PlayerStateMachine ctx, float deltaTime)
    {
        //Normal state has no timer-based behavior
    }
}