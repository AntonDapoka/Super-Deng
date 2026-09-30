using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimationManagerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuFadeManagerScript fadeManager;

    [Header("Distance Settings")]
    [SerializeField] private float distanceMoveImage = 900f;
    [SerializeField] private float distanceMoveButtons = 300f;


    [Header("Duration Settings")]
    [SerializeField] private float durationMoveImage = 2f;
    [SerializeField] private float durationMoveButtons = 1f;
    [SerializeField] private float durationBetweenButtons = 1f;
    [SerializeField] private float durationPanelFade;

    [Header("UI Curves")]
    [SerializeField] private AnimationCurve curveMoveSettings;
    [SerializeField] private AnimationCurve curvePanelFade;

    private void MovePanel(Image image, bool isShown)
    {
        if (image != null) 
            StartCoroutine(MoveUIObject(image.gameObject, distanceMoveImage, durationMoveImage, isShown, true, isShown));
    }

    private IEnumerator ChangeButtons(Button[] buttonsToHide, Button[] buttonsToShow)
    {
        foreach (Button button in buttonsToHide)
            StartCoroutine(MoveUIObject(button.gameObject, distanceMoveButtons, durationMoveButtons, false, true));

        yield return new WaitForSeconds(durationBetweenButtons);

        foreach (Button button in buttonsToShow)
            StartCoroutine(MoveUIObject(button.gameObject, distanceMoveButtons, moveButtonsDuration, true, false));

        float max = Math.Max(moveImagesDuration, 2 * moveButtonsDuration);

        yield return new WaitForSeconds(max - timeWait);
    }

    public void HideButtons(Button[] buttons)
    {
        foreach (Button button in buttons)
            StartCoroutine(MoveUIObject(button.gameObject, distanceMoveButtons, moveButtonsDuration, false, true));
    }

    private IEnumerator MoveUIObject(GameObject obj, float distance, float duration, bool isShown, bool isActiveBefore, bool IsActiveAfter)
    {
        obj.SetActive(isActiveBefore);

        var rectTransform = obj.GetComponent<RectTransform>();
        int positionMultiplier = isShown ? 1 : -1;

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = startPosition + positionMultiplier * distance * Vector2.down;

        yield return AnimatePosition(rectTransform, startPosition, targetPosition, duration);

        obj.SetActive(IsActiveAfter);
    }

    private IEnumerator AnimatePosition(RectTransform rectTransform, Vector2 start, Vector2 target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float progress = moveSettingsCurve.Evaluate(t);

            rectTransform.anchoredPosition = Vector2.Lerp(start, target, progress);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = target;
    }
}
