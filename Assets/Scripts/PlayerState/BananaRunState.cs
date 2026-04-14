using UnityEngine;

public class BananaRunState : IPlayerState
{
    private float duration;
    private float timer;
    private float slowMultiplier = 0.4f;
    private Color color;

    public BananaRunState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(PlayerStateMachine ctx)
    {
        timer = duration;
        color = ctx.GetComponent<SpriteRenderer>().color;
        ctx.GetComponent<SpriteRenderer>().color = Color.lawnGreen;
        ctx.Player.SetMoveSpeedMultiplier(slowMultiplier);
        
        Debug.Log("Banana slip applied");
    }

    public void Exit(PlayerStateMachine ctx)
    {
        ctx.GetComponent<SpriteRenderer>().color = color;
        ctx.Player.ResetStatsToBase();
    }

    // Update is called once per frame
    public void Update(PlayerStateMachine ctx, float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0f)
        {
            ctx.ChangeState(new NormalState());
        }
    }
}
