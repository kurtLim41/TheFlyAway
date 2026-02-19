using UnityEngine;

public class ShrinkState : IPlayerState
{
    private float duration;
    private float timer;
    private Vector3 originalScale;
    private Vector3 shrinkScale;

    public ShrinkState(float duration, Vector3 shrinkScale)
    {
        this.duration = duration;
        this.shrinkScale = shrinkScale;
    }
    public void Enter(PlayerStateMachine ctx)
    {
        timer = duration;
        // Save the original scale to restore later
        originalScale = ctx.transform.localScale;
        ctx.transform.localScale = shrinkScale;
    }

    public void Exit(PlayerStateMachine ctx)
    {
        // Restore original size
        ctx.transform.localScale = originalScale;
    }

    public void Update(PlayerStateMachine ctx, float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0f)
        {
            // Gooes back to normal when boost expires
            ctx.ChangeState(new NormalState());
        }
    }
}
