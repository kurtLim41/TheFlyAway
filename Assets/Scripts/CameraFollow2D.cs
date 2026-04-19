using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public bool followTarget = true;
    
    [Header("Target")]
    public Transform target;

    [Tooltip("The id of the player you want the camera to follow")]
    [Range(1, 2)] public int playerId = 1;

    [Header("Follow")]
    [Tooltip("How quickly the camera reaches the target position (0 = instant).")]
    public float smoothTime = 0.15f;

    [Tooltip("Offset from the player (e.g., keep a bit above). Z is ignored.")]
    public Vector2 offset = new Vector2(0f, 1.5f);

    [Header("Optional: Clamp to level bounds")]
    public bool clampToBounds = false;
    public Vector2 minBounds; // bottom-left world coordinate
    public Vector2 maxBounds; // top-right world coordinate

    private Vector3 _velocity;

    void LateUpdate()
    {
        if (!target || !followTarget) return;

        // Desired camera pos (keep current Z)
        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        // Smooth follow
        Vector3 pos = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);

        var cam = GetComponent<Camera>();
        if (clampToBounds && cam != null)
        {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            pos.x = Mathf.Clamp(pos.x, minBounds.x + horzExtent, maxBounds.x - horzExtent);
            pos.y = Mathf.Clamp(pos.y, minBounds.y + vertExtent, maxBounds.y - vertExtent);
        }


        transform.position = pos;
    }
    
    // Helper method to toggle follow
    public void SetFollow(bool follow)
    {
        followTarget = follow;
    }
}