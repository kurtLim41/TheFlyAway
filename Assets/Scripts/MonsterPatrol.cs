using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterPatrol : MonoBehaviour
{
    [Header("Two-Point (A <-> B)")]
    public bool useLocalOffsets = true;
    public Vector2 pointA = new Vector2(-2f, 0f);
    public Vector2 pointB = new Vector2( 2f, 0f);

    [Header("Movement")]
    public float speed = 2.5f;
    public float arriveThreshold = 0.08f;   // a hair bigger helps avoid jitter
    public float waitAtPoint = 0.1f;
    public bool startAtClosestPoint = true; // NEW: pick A/B based on spawn position

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public bool flipSpriteOnX = true;

    Rigidbody2D rb;
    Vector2 startPos;
    Vector2 aWorld, bWorld;
    int index = 0;      // 0 = A, 1 = B
    bool waiting = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
        ResolveWorldPoints();

        if (startAtClosestPoint)
            index = (Vector2.Distance(startPos, aWorld) <= Vector2.Distance(startPos, bWorld)) ? 0 : 1;

        ClampStartIndex();
    }

    void OnValidate()
    {
        if (speed < 0f) speed = 0f;
        if (arriveThreshold < 0f) arriveThreshold = 0f;
        if (waitAtPoint < 0f) waitAtPoint = 0f;
    }

    void ResolveWorldPoints()
    {
        if (useLocalOffsets)
        {
            aWorld = (Vector2)startPos + pointA;
            bWorld = (Vector2)startPos + pointB;
        }
        else
        {
            aWorld = pointA;
            bWorld = pointB;
        }
    }

    void FixedUpdate()
    {
        if (waiting) return;

        Vector2 pos = rb.position;
        Vector2 target = (index == 0) ? aWorld : bWorld;
        Vector2 toTarget = target - pos;

        // Arrived? Snap and wait -> prevents oscillation
        if (toTarget.sqrMagnitude <= arriveThreshold * arriveThreshold)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            rb.MovePosition(target);                  // snap exactly to the point
            StartCoroutine(AdvanceAfterWait());
            return;
        }

        // Move toward target
        Vector2 step = toTarget.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos + step);

        // Flip sprite visually
        if (spriteRenderer && flipSpriteOnX && Mathf.Abs(step.x) > 0.0001f)
            spriteRenderer.flipX = (step.x < 0f);
    }

    IEnumerator AdvanceAfterWait()
    {
        waiting = true;
        if (waitAtPoint > 0f)
            yield return new WaitForSeconds(waitAtPoint);

        index = 1 - index; // toggle A <-> B
        waiting = false;
    }

    void ClampStartIndex()
    {
        if (index < 0 || index > 1) index = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var respawn = collision.collider.GetComponent<PlayerRespawn>();
            if (respawn) respawn.KillAndRespawn();
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector3 basePos = Application.isPlaying ? (Vector3)startPos : transform.position;
        Vector2 A = useLocalOffsets ? (Vector2)basePos + pointA : pointA;
        Vector2 B = useLocalOffsets ? (Vector2)basePos + pointB : pointB;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(A, 0.08f);
        Gizmos.DrawSphere(B, 0.08f);
        Gizmos.DrawLine(A, B);
    }
#endif
}
