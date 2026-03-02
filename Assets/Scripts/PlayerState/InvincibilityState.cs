using System.Collections;
using UnityEngine;

namespace PlayerState
{
    public class InvincibilityState : IPlayerState
    {
        private float duration;
        private float timer;

        public InvincibilityState(float duration)
        {
            this.duration = duration;
        }
        public void Enter(PlayerStateMachine ctx)
        {
            timer = duration;
            
            ctx.GetComponent<SpriteRenderer>().color = Color.deepPink;
            
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer("player"),
                LayerMask.NameToLayer("enemy"),
                true
            );

            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer("player"),
                LayerMask.NameToLayer("hazard"),
                true
            );
        }

        public void Exit(PlayerStateMachine ctx)
        {
            ctx.GetComponent<SpriteRenderer>().color = Color.white;
            
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer("player"),
                LayerMask.NameToLayer("enemy"),
                false
            );

            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer("player"),
                LayerMask.NameToLayer("hazard"),
                false
            );
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
