using PlayerState;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Ability{
    Shrink,
    Invincibility,
    SpeedBoost,
    Banana,
    LowGravity
}

public class SpecialAbilityPickup : MonoBehaviour
{
    public Ability ability;
    
    [SerializeField] private float duration = 10f;
    [SerializeField] private Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float lowGravityScale = 0.5f;
    [SerializeField] private float speedBooster = 1.5f;
    
    private PlayerStateMachine stateMachine;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        stateMachine = other.GetComponent<PlayerStateMachine>();
        if (stateMachine != null)
        {
            int pickerId = other.GetComponent<PlayerIdentity>()?.playerId ?? 1;
            ChooseStateChange(ability, pickerId);
        }

        //removes the pickup from the scene
        Destroy(gameObject);
    }

    private void ChooseStateChange(Ability ability, int pickerId)
    {
        switch (ability)
        {
            case Ability.Shrink:
                stateMachine.ChangeState(new ShrinkState(duration, shrinkScale));
                break;
            case Ability.Invincibility:
                stateMachine.ChangeState(new InvincibilityState(duration));
                break;
            case Ability.SpeedBoost:
                stateMachine.ChangeState(new SpeedBoostState(duration, speedBooster));
                break;
            case Ability.LowGravity:
                stateMachine.ChangeState(new LowGravityState(duration, lowGravityScale));
                break;
            case Ability.Banana:
                PlayerIdentity targetIdentity = null;
                var allPlayers = FindObjectsOfType<PlayerIdentity>();
                foreach (var p in allPlayers)
                {
                    if (p.playerId != pickerId)
                    {
                        targetIdentity = p;
                        break;
                    }
                }
                if (targetIdentity != null)
                {
                    var targetStateMachine = targetIdentity.GetComponent<PlayerStateMachine>();
                    if (targetStateMachine != null)
                    {
                        targetStateMachine.ChangeState(new BananaRunState(duration));
                    }
                }
                break;
        }
    }
}
