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

    private int _pickupsRemaining;
    private GameObject _currentPickupGO;

    private void Start()
    {
        _pickupsRemaining = maxPickups;
        SpawnPickup();
    }

    private void SpawnPickup()
    {
        if (_pickupsRemaining <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (pickupPrefabs == null || pickupPrefabs.Length == 0)
        {
            Debug.LogError("[RandomAbilitySpawner] No prefabs in pool! Add prefabs to the Pickup Prefabs list.", this);
            return;
        }

        // Pick a random prefab from the pool
        GameObject chosen = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
        if (chosen == null)
        {
            Debug.LogError("[RandomAbilitySpawner] A null entry is in the prefab pool.", this);
            return;
        }

        _currentPickupGO = Instantiate(chosen, transform.position, Quaternion.identity);

        var watcher = _currentPickupGO.AddComponent<PickupCollectedWatcher>();
        watcher.OnCollected += HandlePickupCollected;

        Debug.Log($"[RandomAbilitySpawner] Spawned '{chosen.name}'. Collections remaining: {_pickupsRemaining}");
    }

    private void HandlePickupCollected()
    {
        _pickupsRemaining--;

        if (_pickupsRemaining <= 0)
        {
            Destroy(gameObject); // done for this level
        }
        else
        {
            Invoke(nameof(SpawnPickup), respawnDelay);
        }
    }

    private void OnDestroy()
    {
        if (_currentPickupGO != null)
            Destroy(_currentPickupGO);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
#endif
}