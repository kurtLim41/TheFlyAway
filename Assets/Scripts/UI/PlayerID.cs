using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [Header("Identiy Colors")]
    public Color playerColor = Color.white;

    [Header("Animation Settings")]
    public float bobbingSpeed = 3f;  // How fast it bobs
    public float bobbingHeight = 0.2f; // How high/low it bobs

    private SpriteRenderer arrowRenderer;
    private ParticleSystem particleSys;
    private Vector3 startPos;

    void Awake()
    {
        arrowRenderer = GetComponentInChildren<SpriteRenderer>();
        particleSys = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        startPos = transform.localPosition;
        
        if (arrowRenderer != null)
        {
            arrowRenderer.color = playerColor;
        }

        if (particleSys != null)
        {
            var mainModule = particleSys.main;
            mainModule.startColor = playerColor;
        }
    }

    void Update()
    {
        // --- 2D Bobbing Motion ---s
        float newY = startPos.y + (Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight);
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}