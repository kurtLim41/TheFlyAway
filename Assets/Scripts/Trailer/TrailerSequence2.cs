using System.Collections;
using UnityEngine;

public class TrailerSequence2 : MonoBehaviour
{
    public CameraFollow2D mainCameraFollow;
    //
    // void Start()
    // {
    //     StartSceneSequence();
    // }
    //
    // void StartSceneSequence()
    // {
    //     // Step 1: follow player
    //     mainCameraFollow.SetFollow(true);
    //
    //     // Step 2: after 2 seconds, stop following
    //     StartCoroutine(StopFollowAfterSeconds(4f));
    // }
    //
    // IEnumerator StopFollowAfterSeconds(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //
    //     // Stop following player
    //     mainCameraFollow.SetFollow(false);
    //
    //     // Optional: start panning up manually
    //     StartCoroutine(PanUp());
    // }
    //
    // IEnumerator PanUp()
    // {
    //     float panDuration = 10f;
    //     float elapsed = 0f;
    //     Vector3 startPos = new Vector3(111f, mainCameraFollow.transform.position.y, mainCameraFollow.transform.position.z);
    //     Vector3 endPos = startPos + new Vector3(0, 50f, 0); // move up 5 units
    //
    //     while (elapsed < panDuration)
    //     {
    //         elapsed += Time.deltaTime;
    //         mainCameraFollow.transform.position = Vector3.Lerp(startPos, endPos, elapsed / panDuration);
    //         yield return null;
    //     }
    // }
    
    public float stopFollowX = 111f;   // X position at which camera stops following
    public float panDistanceY = 70f;   // how far to pan up
    public float panDuration = 10f;

    void Start()
    {
        StartSceneSequence();
    }

    void StartSceneSequence()
    {
        // Step 1: follow player
        mainCameraFollow.SetFollow(true);

        // Step 2: watch for X position
        StartCoroutine(StopFollowAtX(stopFollowX));
    }

// Coroutine to stop follow once camera reaches target X
    IEnumerator StopFollowAtX(float targetX)
    {
        // Wait until camera's X reaches target
        while (mainCameraFollow.transform.position.x < targetX)
        {
            yield return null; // wait a frame
        }

        // Stop following player
        mainCameraFollow.SetFollow(false);

        // Start pan up
        StartCoroutine(PanUp());
    }

    IEnumerator PanUp()
    {
        float elapsed = 0f;
        Vector3 startPos = mainCameraFollow.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, panDistanceY, 0f);

        while (elapsed < panDuration)
        {
            elapsed += Time.deltaTime;
            mainCameraFollow.transform.position = Vector3.Lerp(startPos, endPos, elapsed / panDuration);
            yield return null;
        }
    }
}
