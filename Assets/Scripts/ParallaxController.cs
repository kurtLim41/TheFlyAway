using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float length, startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffectX;
    public float parallaxEffectY;

    void Start()
    {
        startPosX = transform.position.x;
        //startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Calculate distance background move based on cam movement
        float distanceX = cam.transform.position.x * parallaxEffectX; // 0 = move with cam || 1 = won't move || 0.5 = half
        float movementX = cam.transform.position.x * (1 - parallaxEffectX);
        
        //float distanceY = cam.transform.position.y * parallaxEffectY;
        
        transform.position = new Vector3(startPosX + distanceX, transform.position.y, transform.position.z);
        
        // if background has reached the end of its length adjust its position for infinite scrolling
        if (movementX > startPosX + length)
        {
            startPosX += length;
        }
        else if (movementX < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
