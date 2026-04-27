using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowPresenterScript : MonoBehaviour, IFlickerable
{
    [SerializeField] protected WindowViewScript windowView;
    [SerializeField] protected FlickeringViewScript flickeringView;
    [SerializeField] protected AnimationCurve heightCurve;
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected Sprite spriteButtonMin;
    [SerializeField] protected Sprite spriteButtonMax;

   public FlickeringViewScript FlickeringView => flickeringView;

   public void Initialize()
    {
        
    }
    public void CloseWindow(WindowComponentsScript components)
    {
        float durationLocal = 0.5f;
        MinimizeWindow(components, 0.33f);

        StartFlickering(components.GetWindowTitleBar().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetWindowSpace().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetWindowSpaceInner().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetIconButtonMinMax().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetIconButtonClose().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetImageButtonClose().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetImageButtonMinMax().GetComponent<Image>(), durationLocal);
        StartFlickering(components.GetWindowSpaceInnerText().GetComponent<TextMeshProUGUI>(), durationLocal);
        StartFlickering(components.GetWindowTitleBarText().GetComponent<TextMeshProUGUI>(), durationLocal);
    }

    private void StartFlickering(Component component, float duration)
    {
        bool isImage = component is Image;

        if (isImage)
        {
            Image image = component as Image;
            Color colorStart = image.color;
            Color transparentColor = image.color;
            transparentColor.a = 0f;
            FlickeringView.StartFlickeringAnimationEffect(image, duration, colorStart, transparentColor, true, true);
        }
        else
        {
            TextMeshProUGUI tmpro = component as TextMeshProUGUI;
            Color colorStart = tmpro.color;
            Color transparentColor = tmpro.color;
            transparentColor.a = 0f;
            FlickeringView.StartFlickeringAnimationEffect(tmpro, duration, colorStart, transparentColor, true, true);
        }
    }

    public void MinimizeWindow(WindowComponentsScript components, float duration = 0.25f)
    {
        RectTransform windowSpace = components.GetWindowSpace();
        WindowSettingsScript windowSettings = components.GetWindowSettings();
        float windowSpaceHeight = windowSettings.GetWindowSpaceHeight();
        Vector2 windowPositionStart = windowSettings.GetWindowPosition();
        Vector2 windowPositionTarget = new(windowPositionStart.x, windowPositionStart.y + windowSpaceHeight / 2f);

        components.GetIconButtonMinMax().sprite = spriteButtonMax;
        StartCoroutine(ChangingWindowHeight(windowSpace, windowPositionStart, windowPositionTarget, windowSpaceHeight, 0f, duration));
    }

    public void MaximizeWindow(WindowComponentsScript components, float duration = 0.25f)
    {
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

   public void SetFlickeringEffect()
   {
      throw new System.NotImplementedException();
   }
}
