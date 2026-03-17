using System;
using UnityEngine;

public class PickupCollectedWatcher : MonoBehaviour
{
    public event Action OnCollected;

    private void OnDestroy()
    {
        OnCollected?.Invoke();
    }
}