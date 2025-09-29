using UnityEngine;

public class TimelineController : MonoBehaviour
{
    [Header("Timeline RectTransform")]
    [SerializeField] private RectTransform timelineContent;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 500f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minWidth = 500f;
    [SerializeField] private float maxWidth = 5000f;

    private void Update()
    {
        if (timelineContent == null)
            return;

        //if (IsPointerOverUI()) return;

        HandleScroll();
        HandleZoom();
    }

    private void HandleScroll()
    {
        if (Input.GetMouseButton(2)) // Middle mouse
        {
            Debug.Log("SHENME1");
            float delta = Input.GetAxis("Mouse X");
            timelineContent.anchoredPosition += new Vector2(delta * scrollSpeed * Time.deltaTime, 0f);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Debug.Log("SHENME2");
            Vector2 size = timelineContent.sizeDelta;
            float newWidth = Mathf.Clamp(size.x * (1f + scroll * zoomSpeed), minWidth, maxWidth);
            timelineContent.sizeDelta = new Vector2(newWidth, size.y);
        }
    }
    /*
    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }*/
}
