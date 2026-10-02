using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class WindowSettingsScript : MonoBehaviour
{
    [SerializeField] private RectTransform windowSpace;
    private float windowSpaceWidth;
    private float windowSpaceHeight;
    private Vector2 windowPosition;

    public float GetWindowSpaceWidth() => windowSpaceWidth;
    public float GetWindowSpaceHeight() => windowSpaceHeight;
    public Vector2 GetWindowPosition() => windowPosition;

   private void Awake()
   {
        windowSpaceWidth = windowSpace.rect.width;
        windowSpaceHeight = windowSpace.rect.height;
        windowPosition = windowSpace.anchoredPosition;
        Debug.Log(windowPosition.y);
   }
}

