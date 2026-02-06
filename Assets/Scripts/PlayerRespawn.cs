using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;

    Rigidbody2D rb;
    Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (spawnPoint == null)
            Debug.LogWarning("PlayerRespawn: spawnPoint not set. Assign your SpawnPoint transform.");
    }

    void Start()
    {
        Respawn(); 
    }

    public void KillAndRespawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        if (col) col.enabled = false;

        //zero velocity and teleport
        if (rb) rb.linearVelocity = Vector2.zero;
        Respawn();

        //small delay to avoid double triggers
        yield return new WaitForSeconds(0.05f);

        if (col) col.enabled = true;
    }

    void Respawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            if (rb) rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Debug.LogError("PlayerRespawn: No spawnPoint assigned.");
        }
    }
}