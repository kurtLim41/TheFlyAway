using UnityEngine;

public class ParallaxSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab; // your parallax background prefab
    public Camera[] cameras; // assign all player cameras in inspector

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            Camera cam = cameras[i];
            
            // Spawn a copy for this camera and make it a child of the camera so it moves with it
            GameObject bgInstance = Instantiate(backgroundPrefab, cam.transform, true);

            // set layer name
            string layerName = "player" + (i + 1) + "BG";
            int layer = LayerMask.NameToLayer(layerName);
            
            SetLayerRecursively(bgInstance, layer);

            // sets camera instance to each parallax controller on the background objects
            ParallaxController[] backgrounds = bgInstance.GetComponentsInChildren<ParallaxController>();

            foreach (ParallaxController background in backgrounds)
            {
                background.cam = cam.gameObject;
            }
            
            // which layers are visible for each camera
            cam.cullingMask = LayerMask.GetMask("Default", "UI", "ground", "player", "enemy", "hazard", layerName);
        }
    }
    
    // Recursive method to apply layers to all child objects
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, newLayer);
    }
}
