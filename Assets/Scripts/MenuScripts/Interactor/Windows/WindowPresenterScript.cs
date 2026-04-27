using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowPresenterScript : MonoBehaviour
{
    [SerializeField] protected WindowViewScript windowView;
    [SerializeField] protected float durationMinimizing = 0.25f;
    [SerializeField] protected AnimationCurve heightCurve;
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected Image imageButtonMinMax;
    [SerializeField] protected Sprite spriteButtonMin;
    [SerializeField] protected Sprite spriteButtonMax;
    
    public void Initialize()
    {
        
    }
    public void CloseWindow()
    {
        
    }

    public void MinimizeWindow(WindowComponentsScript components)
    {
        RectTransform windowSpace = components.GetWindowSpace();
        WindowSettingsScript windowSettings = components.GetWindowSettings();
        float windowSpaceHeight = windowSettings.GetWindowSpaceHeight();
        Vector2 windowPositionStart = windowSettings.GetWindowPosition();
        Vector2 windowPositionTarget = new(windowPositionStart.x, windowPositionStart.y + windowSpaceHeight / 2f);

        imageButtonMinMax.sprite = spriteButtonMax;
        StartCoroutine(ChangingWindowHeight(windowSpace, windowPositionStart, windowPositionTarget, windowSpaceHeight, 0f));
    }

    public void MaximizeWindow(WindowComponentsScript components)
    {
        RectTransform windowSpace = components.GetWindowSpace();
        WindowSettingsScript windowSettings = components.GetWindowSettings();
        float windowSpaceHeight = windowSettings.GetWindowSpaceHeight();
        Vector2 windowPositionTarget = windowSettings.GetWindowPosition();
        Vector2 windowPositionStart = new(windowPositionTarget.x, windowPositionTarget.y + windowSpaceHeight / 2f);

        imageButtonMinMax.sprite = spriteButtonMin;
        StartCoroutine(ChangingWindowHeight(windowSpace, windowPositionStart, windowPositionTarget, 0f, windowSpaceHeight));
    }

    private IEnumerator ChangingWindowHeight(RectTransform windowSpace, Vector2 positionStart, Vector2 positionTarget, float heightStart, float heightTarget)
    {
        float time = 0f;
        Vector2 size = windowSpace.sizeDelta;

        while (time < durationMinimizing)
        {
            time += Time.deltaTime;
            float t = time / durationMinimizing;

            float curvedHeightT = heightCurve.Evaluate(t);
            float newHeight = Mathf.Lerp(heightStart, heightTarget, curvedHeightT);
            windowSpace.sizeDelta = new Vector2(size.x, newHeight);

            float curvedPositionT = positionCurve.Evaluate(t);
            Vector2 newPosition = Vector2.Lerp(positionStart, positionTarget, curvedPositionT);
            windowSpace.anchoredPosition = newPosition;

            yield return null;
        }
        windowSpace.sizeDelta = new Vector2(size.x, heightTarget);
        windowSpace.anchoredPosition = positionTarget;
    }
}
