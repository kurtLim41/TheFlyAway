using System.Collections;
using UnityEngine;

namespace PlayerState
{
    public class InvincibilityState : IPlayerState
    {
        private float duration;
        private float timer;
        private Color color;
        
        private bool startedFlicker = false;
        private Coroutine flickerRoutine;

        public InvincibilityState(float duration)
        {
            this.duration = duration;
        }
        public void Enter(PlayerStateMachine ctx)
        {
            timer = duration;
            
            color = ctx.GetComponent<SpriteRenderer>().color;
            ctx.GetComponent<SpriteRenderer>().color = Color.deepPink;
            
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(ctx.LayerName),
                LayerMask.NameToLayer("enemy"),
                true
            );

            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(ctx.LayerName),
                LayerMask.NameToLayer("hazard"),
                true
            );
        }

        public void Exit(PlayerStateMachine ctx)
        {
            if (flickerRoutine != null)
                ctx.StopCoroutine(flickerRoutine);
            
            ctx.GetComponent<SpriteRenderer>().color = color;
            
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(ctx.LayerName),
                LayerMask.NameToLayer("enemy"),
                false
            );

            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(ctx.LayerName),
                LayerMask.NameToLayer("hazard"),
                false
            );
        }

        public void Update(PlayerStateMachine ctx, float deltaTime)
        {
            timer -= deltaTime;
            
            if (!startedFlicker && timer <= 2f)
            {
                startedFlicker = true;
                flickerRoutine = ctx.StartCoroutine(FlickerShield(ctx));
            }
            
            if (timer <= 0f)
            {
                // Goes back to normal when boost expires
                ctx.ChangeState(new NormalState());
            }
        }
        
        IEnumerator FlickerShield(PlayerStateMachine ctx)
        {
            SpriteRenderer sr = ctx.GetComponent<SpriteRenderer>();

            while (timer > 0f)
            {
                sr.color = color;
                yield return new WaitForSeconds(0.2f);

                sr.color = Color.deepPink;
                yield return new WaitForSeconds(0.2f);
            }

            // ensure final color is correct
            sr.color = color;
        }
    }
}
