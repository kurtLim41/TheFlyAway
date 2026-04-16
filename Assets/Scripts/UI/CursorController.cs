using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D hoverCursor;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        SetNormalCursor();
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }

    public void SetHoverCursor()
    {
        Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    }
}