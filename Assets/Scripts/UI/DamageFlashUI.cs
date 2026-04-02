using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    // Notice the 'static instance' is gone!
    
    [Header("Flash Settings")]
    public Image flashImage;                                 
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);   
    public float fadeSpeed = 3f;                             

    void Awake()
    {
        // Ensure it starts completely invisible
        if (flashImage != null) flashImage.color = Color.clear;
    }

    public void FlashScreen()
    {
        if (flashImage != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        flashImage.color = flashColor;

        while (flashImage.color.a > 0.01f)
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        
        flashImage.color = Color.clear;
    }
}