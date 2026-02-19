using UnityEngine;

public class ShrinkPickup : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 0.5f);
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stateMachine = other.GetComponent<PlayerStateMachine>();
        if (stateMachine != null)
        {
            stateMachine.ChangeState(new ShrinkState(duration, shrinkScale));
        }

        //removes the pickup from the scene
        Destroy(gameObject);
    }
}
