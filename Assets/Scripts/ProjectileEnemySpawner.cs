

using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class ProjectileEnemySpawner : MonoBehaviour
{
    [Header("Projectile Prefab")] public SparkProjectile2D sparkPrefab;
    [Header("Manual Spawn Points")] public Transform[] spawnPoints;

    [Header("Random timing")]
    public float minDelay = 1.0f;
    public float maxDelay = 3.0f;

    [Header("Random amount per burst")] 
    public int minPerBurst = 1;
    public int maxPerBurst = 3;
    
    [Header("Random Speed")]
    public float minSpeed = 6f;
    public float maxSpeed = 13f;
    
    [Header("Direction")]
    public Vector2 baseDirection = Vector2.left;
    public float directionRandomAngleDegrees = 15f;

    private Coroutine _loop;

    void OnEnable() => _loop = StartCoroutine(Loop());
    void OnDisable() { if (_loop != null) StopCoroutine(_loop); }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            SpawnBurst();
        }
    }

    void SpawnBurst()
    {
        if (!sparkPrefab || spawnPoints == null || spawnPoints.Length == 0) return;
        
        int count = Random.Range(minPerBurst, maxPerBurst + 1);

        for (int i = 0; i < count; i++)
        {
            var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var spark = Instantiate(sparkPrefab, sp.position, Quaternion.identity);
            
            float speed = Random.Range(minSpeed, maxSpeed);
            Vector2 dir = RandomDir(baseDirection, directionRandomAngleDegrees);
            
            spark.Initialize(dir, speed);
        }
    }

    Vector2 RandomDir(Vector2 baseDir, float maxAngleDeg)
    {
        float angle = Random.Range(-maxAngleDeg, maxAngleDeg);
        return (Quaternion.Euler(0, 0, angle) * baseDir.normalized);
    }
}