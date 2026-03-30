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

        // Save original scale
        originalScale = ctx.transform.localScale;

        // Keep the current facing direction by preserving the X sign
        float xSign = Mathf.Sign(originalScale.x);
        if (xSign == 0f) xSign = 1f;

        ctx.transform.localScale = new Vector3(
            Mathf.Abs(shrinkScale.x) * xSign,
            Mathf.Abs(shrinkScale.y),
            Mathf.Abs(shrinkScale.z)
        );
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
            // Goes back to normal when boost expires
            ctx.ChangeState(new NormalState());
        }
    }
}
