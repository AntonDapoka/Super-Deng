using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Menu.Effects.Flickering;

public class WindowPresenterScript : MonoBehaviour
{
    //[SerializeField] protected WindowViewScript windowView;
    [SerializeField] protected FlickeringPresenterScript flickeringPresenter;
    [SerializeField] protected AnimationCurve heightCurve;
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected Sprite spriteButtonMin;
    [SerializeField] protected Sprite spriteButtonMax;

    [Header("Durations")]
    [SerializeField, Min(0f)] private float closeDuration = 0.5f;
    [SerializeField, Min(0f)] private float closeMinimizeDuration = 0.33f;
    [SerializeField, Min(0f)] private float minimizeDuration = 0.25f;
    [SerializeField, Min(0f)] private float maximizeDuration = 0.25f;

    public void Initialize()
    {
        
    }
    public void CloseWindow(WindowComponentsScript components)
    {
        MinimizeWindow(components, closeMinimizeDuration);

        StartFlickering(components.GetWindowTitleBar().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetWindowSpace().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetWindowSpaceInner().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetIconButtonMinMax().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetIconButtonClose().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetImageButtonClose().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetImageButtonMinMax().GetComponent<Image>(), closeDuration);
        StartFlickering(components.GetWindowSpaceInnerText().GetComponent<TextMeshProUGUI>(), closeDuration);
        StartFlickering(components.GetWindowTitleBarText().GetComponent<TextMeshProUGUI>(), closeDuration);
    }

    private void StartFlickering(Graphic graphic, float duration)
    {
        Color initialColor = graphic.color;
        Color transparentColor = initialColor;
        transparentColor.a = 0f;

        flickeringPresenter.Flicker(graphic, initialColor, transparentColor, duration, isTurningOn: true, isBlinking: true, isDeactivateTargetAfter: true);
    }

    public void MinimizeWindow(WindowComponentsScript components)
    {
        MinimizeWindow(components, minimizeDuration);
    }

    private void MinimizeWindow(WindowComponentsScript components, float duration)
    {
        RectTransform windowSpace = components.GetWindowSpace();
        WindowSettingsScript windowSettings = components.GetWindowSettings();
        float windowSpaceHeight = windowSettings.GetWindowSpaceHeight();
        Vector2 windowPositionStart = windowSettings.GetWindowPosition();
        Vector2 windowPositionTarget = new(windowPositionStart.x, windowPositionStart.y + windowSpaceHeight / 2f);

        components.GetIconButtonMinMax().sprite = spriteButtonMax;
        StartCoroutine(ChangingWindowHeight(windowSpace, windowPositionStart, windowPositionTarget, windowSpaceHeight, 0f, duration));
    }

    public void MaximizeWindow(WindowComponentsScript components)
    {
        float duration = maximizeDuration;

        RectTransform windowSpace = components.GetWindowSpace();
        WindowSettingsScript windowSettings = components.GetWindowSettings();
        float windowSpaceHeight = windowSettings.GetWindowSpaceHeight();
        Vector2 windowPositionTarget = windowSettings.GetWindowPosition();
        Vector2 windowPositionStart = new(windowPositionTarget.x, windowPositionTarget.y + windowSpaceHeight / 2f);

        components.GetIconButtonMinMax().sprite = spriteButtonMin;
        StartCoroutine(ChangingWindowHeight(windowSpace, windowPositionStart, windowPositionTarget, 0f, windowSpaceHeight, duration));
    }

    private IEnumerator ChangingWindowHeight(RectTransform windowSpace, Vector2 positionStart, Vector2 positionTarget, float heightStart, float heightTarget, float duration)
    {
        float time = 0f;
        Vector2 size = windowSpace.sizeDelta;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

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
