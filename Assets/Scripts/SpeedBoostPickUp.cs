using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private int extraJumps = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stateMachine = other.GetComponent<PlayerStateMachine>();
        if (stateMachine != null)
        {
            stateMachine.ChangeState(new SpeedBoostState(duration, speedMultiplier, extraJumps));
        }

        //removes the pickup from the scene
        Destroy(gameObject);
    }
}