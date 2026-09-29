using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimationManagerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuFadeManagerScript fadeManager;

    [Header("Settings")]
    [SerializeField] private float moveImagesDuration = 2f;
    [SerializeField] private float moveButtonsDuration = 1f;
    [SerializeField] private float waitBetweenButtons = 1f;
    [SerializeField] private float panelFadeDuration;

    [Header("UI Curves")]
    [SerializeField] private AnimationCurve moveSettingsCurve;
    [SerializeField] private AnimationCurve panelFadeCurve;

    public void HideButtons(Button[] buttons)
    {
        
    }

    private IEnumerator SetImageChangeButtons(Image image, Button[] buttonsMain, float timeWait, Button[] buttonsExtra, bool isImageUp, bool isInteractWithLogo)
    {
       // wall.gameObject.SetActive(true);

        //if (isInteractWithLogo) MLNFS.LogoTurningOnAndOff(moveImagesDuration, isImageUp, true, isImageUp, true, false, true, 0.1f, 0.4f);
        if (image != null) 
            StartCoroutine(MoveObjectAndUI(image.gameObject, 900f * (isImageUp ? -1 : 1), moveImagesDuration, true, true));

        foreach (Button button in buttonsMain)
        {
            StartCoroutine(MoveObjectAndUI(button.gameObject, 300f, moveButtonsDuration, false, true));
        }
        yield return new WaitForSeconds(timeWait);

        foreach (Button button in buttonsExtra)
        {
            StartCoroutine(MoveObjectAndUI(button.gameObject, 300f, moveButtonsDuration, true, false));
        }

        float max = System.Math.Max(moveImagesDuration, 2* moveButtonsDuration);

        yield return new WaitForSeconds(max - timeWait);
        //wall.gameObject.SetActive(false);
    }

    private IEnumerator MoveObjectAndUI(GameObject obj, float bias, float duration, bool isChosen, bool isDown)
    {
        if (isChosen) obj.SetActive(true);
        
        float elapsedTime = 0f;

        int positionMultiplier = isDown ? 1 : -1;

        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        Vector2 initialPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            float curveProgress = moveSettingsCurve.Evaluate(elapsedTime / duration);

            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, initialPos - new Vector2(0, bias * positionMultiplier), curveProgress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = initialPos - new Vector2(0, bias) * positionMultiplier;
        
        if (!isChosen) obj.SetActive(false);
    }
}
