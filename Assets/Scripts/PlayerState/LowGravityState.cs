namespace PlayerState
{
    public class LowGravityState : IPlayerState
    {
        private float duration;
        private float timer;
        private float lowGravityScale;

        public LowGravityState(float duration, float lowGravityScale)
        {
            this.duration = duration;
            this.lowGravityScale = lowGravityScale;
        }
        public void Enter(PlayerStateMachine ctx)
        {
            timer = duration;
            ctx.Player.SetGravityScaleMultiplier(lowGravityScale);
        }

        public void Exit(PlayerStateMachine ctx)
        {
            ctx.Player.ResetStatsToBase();
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
}
