using UnityEngine;

public class RandomAbilitySpawner : MonoBehaviour
{
    [Header("Pickup Prefabs Pool")]
    [Tooltip("Drag any pickup prefabs here (SpeedBoostItem, SpecialAbilityPickup, etc). One is chosen at random each spawn.")]
    [SerializeField] private GameObject[] pickupPrefabs;

    [Header("Spawn Settings")]
    [Tooltip("How many times a pickup can be collected before this spawner destroys itself.")]
    [SerializeField] private int maxPickups = 2;

    [Tooltip("Seconds between a pickup being collected and the next one spawning.")]
    [SerializeField] private float respawnDelay = 2f;

    private int pickupsSpawnedSoFar;
    private GameObject _currentPickup;

    private void Start()
    {
        pickupsSpawnedSoFar = maxPickups;
        SpawnPickup();
    }

    private void SpawnPickup()
    {
        if (pickupsSpawnedSoFar <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (pickupPrefabs == null || pickupPrefabs.Length == 0)
        {
            Debug.LogError("[RandomAbilitySpawner] No pickup prefabs assigned", this);
            return;
        }

        // Pick a random prefab from the pool
        GameObject chosen = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
        if (chosen == null)
        {
            Debug.LogError("[RandomAbilitySpawner] A null entry is in the prefab pool.", this);
            return;
        }
        
        _currentPickup = Instantiate(chosen, transform.position, Quaternion.identity, transform);
        
        var watcher = _currentPickup.GetComponent<PickupCollectedWatcher>();
        if (watcher == null)
        {
            watcher = _currentPickup.AddComponent<PickupCollectedWatcher>();
        }

        watcher.OnCollected -= HandlePickupCollected;
        watcher.OnCollected += HandlePickupCollected;
        
    }

    private void HandlePickupCollected()
    {
        if (_currentPickup != null)
        {
            Destroy(_currentPickup);
            _currentPickup = null; // done for this level
        }
        
        pickupsSpawnedSoFar--;
        
        if (pickupsSpawnedSoFar <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Invoke(nameof(SpawnPickup), respawnDelay);
        }
    }

    private void OnDestroy()
    {
        if (_currentPickup != null)
        {
            Destroy(_currentPickup);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
#endif
}