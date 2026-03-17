using UnityEngine;

public class ItemHover : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverSpeed = 3f;     // How fast it bobs up and down
    public float hoverHeight = 0.2f;  // How high and low it goes

    [Header("Pulse Settings")]
    public bool enablePulse = true;
    public float pulseSpeed = 4f;     // How fast it shrinks and grows
    public float pulseSize = 0.1f;    // How much it expands

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        // 1. The Floating Math (Sine wave)
        float newY = startPos.y + (Mathf.Sin(Time.time * hoverSpeed) * hoverHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // 2. The Pulsing Math
        if (enablePulse)
        {
            float currentSize = 1f + (Mathf.Sin(Time.time * pulseSpeed) * pulseSize);
            transform.localScale = startScale * currentSize;
        }
    }
}