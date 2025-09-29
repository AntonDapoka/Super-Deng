using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindowScript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private bool HaveTexts;
    [SerializeField] private bool HaveImages;


    private RectTransform dragRectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        if (dragRectTransform == null)
        {
            dragRectTransform = transform.parent.GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null || dragRectTransform == null || canvasRectTransform == null) return;

        Vector2 delta = eventData.delta / canvas.scaleFactor;
        dragRectTransform.anchoredPosition += delta;

        ClampToWindow();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }

    public void SetCanvas(Canvas newCanvas)
    {
        canvas = newCanvas;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    private void ClampToWindow()
    {
        Vector2 pos = dragRectTransform.anchoredPosition;
        Vector2 size = dragRectTransform.rect.size;
        Vector2 canvasSize = canvasRectTransform.rect.size;

        // Ограничения с учётом pivot
        float minX = -canvasSize.x * 0.5f + size.x * dragRectTransform.pivot.x;
        float maxX = canvasSize.x * 0.5f - size.x * (1 - dragRectTransform.pivot.x);
        float minY = -canvasSize.y * 0.5f + size.y * dragRectTransform.pivot.y;
        float maxY = canvasSize.y * 0.5f - size.y * (1 - dragRectTransform.pivot.y);

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        dragRectTransform.anchoredPosition = pos;
    }

    
}
