using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;
        public DamageFlashUI myScreenFlash; 
        public CameraShake myCameraShake; // <-- Add this new line!
        
    Rigidbody2D rb;
    Collider2D col;
    
    private int id;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        id = GetComponent<PlayerIdentity>().playerId;
        
        if (spawnPoint == null)
            Debug.LogWarning("PlayerRespawn: spawnPoint not set. Assign your SpawnPoint transform.");
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetSpawnPoint();
        SetCameraFollow();
    }

    private void SetSpawnPoint()
    {
        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        
        if (spawns.Length > 0)
        {
            foreach (var spawn in spawns)
            {
                if (spawn.playerId == id)
                {
                    spawnPoint = spawn.transform;
                    Respawn();
                    break;
                }
            }
        }
    }

    private void SetCameraFollow()
    {
        CameraFollow2D[] cams = FindObjectsByType<CameraFollow2D>(FindObjectsSortMode.None);

        if (cams.Length > 0)
        {
            foreach (var cam in cams)
            {
                if (cam.playerId == id)
                {
                    cam.target = transform;
                    break;
                }
            }
        }
    }
}