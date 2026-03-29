using Trailer;
using UnityEngine;

public class TrailerIntroSequence : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsWallClimbing = Animator.StringToHash("IsWallClimbing");
    
    public Transform player1;
    public Transform player2;
    
    public Animator player1Animator;
    public Animator player2Animator;

    public float speed = 100f;
    public float baseY = 0f;

    private float time;

    void Start()
    {
        TrailerMode.IsActive = true;
        // Optional: disable player input scripts
        DisablePlayerControl(player1);
        DisablePlayerControl(player2);
        SetUpAnimators();
    }

    void Update()
    {
        time += Time.deltaTime;

        float x = speed * time;

        // Player 1 (straight run)
        player1.position = new Vector3(x, baseY, 0);

        // Player 2 behavior
        float delay = 3f;
        float yOffset = -1f;

        float xOffset;

        if (time < 5.5f)
        {
            float t = Mathf.Clamp01((time - delay) / 2.5f);
            xOffset = Mathf.Lerp(-20f, 0.3f, Mathf.SmoothStep(0, 1, t));
        }
        else if (time < 6.5f)
        {
            xOffset = 0.2f; // run side-by-side
        }
        else
        {
            float t = Mathf.Clamp01((time - 6.5f) / 2.0f);
            xOffset = Mathf.Lerp(0.2f, 20f, Mathf.SmoothStep(0, 1, t));
        }

        player2.position = new Vector3(x + xOffset, baseY + yOffset, 0);
    }

    void DisablePlayerControl(Transform player)
    {
        var controller = player.GetComponent<MonoBehaviour>(); 
        if (controller != null)
            controller.enabled = false;
    }

    void SetUpAnimators()
    {
        player1Animator.SetFloat(Speed, 100f);
        player2Animator.SetFloat(Speed, 100f);
        
        player1Animator.SetBool(IsGrounded, true);
        player2Animator.SetBool(IsGrounded, true);
        
        player1Animator.SetBool(IsWallClimbing, false);
        player2Animator.SetBool(IsWallClimbing, false);
    }
}
