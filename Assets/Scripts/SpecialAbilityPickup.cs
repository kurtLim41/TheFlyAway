using PlayerState;
using UnityEngine;

public enum Ability{
    Shrink,
    Invincibility,
    SpeedBoost,
    Obstacle,
    LowGravity
}

public class SpecialAbilityPickup : MonoBehaviour
{
    public Ability ability;
    
    [SerializeField] private float duration = 10f;
    [SerializeField] private Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 0.5f);
    
    private PlayerStateMachine stateMachine;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        stateMachine = other.GetComponent<PlayerStateMachine>();
        if (stateMachine != null)
        {
            ChooseStateChange(ability);
        }

        //removes the pickup from the scene
        Destroy(gameObject);
    }

    private void ChooseStateChange(Ability ability)
    {
        switch (ability)
        {
            case Ability.Shrink:
                stateMachine.ChangeState(new ShrinkState(duration, shrinkScale));
                break;
            case Ability.Invincibility:
                stateMachine.ChangeState(new InvincibilityState(duration));
                break;
            case Ability.LowGravity:
                stateMachine.ChangeState(new LowGravityState(duration));
                break;
        }
    }
}
