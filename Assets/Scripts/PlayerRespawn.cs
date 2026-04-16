using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("Juice & Effects")]
    public DamageFlashUI myScreenFlash; 
    public CameraShake myCameraShake;
    public AudioClip deathSound;
    
    private AudioSource myAudioSource;
    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        myAudioSource = GetComponent<AudioSource>();

        if (spawnPoint == null)
        {
            Debug.LogWarning("PlayerRespawn: spawnPoint not set.");
        }
    }

    void Start()
    {
        Respawn(); 
    }

    public void KillAndRespawn()
    {
        if (myScreenFlash != null)
        {
            myScreenFlash.FlashScreen();
        }

        if (myCameraShake != null)
        {
            myCameraShake.TriggerShake(0.2f, 0.4f);
        }

        if (myAudioSource != null && deathSound != null)
        {
            myAudioSource.PlayOneShot(deathSound, 1.0f);
        }

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        if (col) col.enabled = false;
        if (rb) rb.linearVelocity = Vector2.zero;

        Respawn();

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