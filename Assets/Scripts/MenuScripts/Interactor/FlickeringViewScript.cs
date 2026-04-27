using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringViewScript : MonoBehaviour
{
    public AnimationCurve colorChangeCurveTurnOn;
    public AnimationCurve colorChangeCurveTurnOff;

    public void StartFlickeringAnimationEffect(Component component, float duration, Color initialColor, Color targetColor, bool isTurningOn, bool isBlinking)
    {
        StartCoroutine(FlickeringEffect(component, duration, initialColor, targetColor, isTurningOn, isBlinking));
    }

    private IEnumerator FlickeringEffect(Component component, float duration, Color initialColor, Color targetColor, bool isTurningOn, bool isBlinking)
    {
        float elapsedTime = 0f;
        float randomTime = 0f;
        int interruptionCount = isBlinking ? Random.Range(0, 2) : 0;

        Color initialColorSafer = initialColor;
        bool isImage = component is Image;
        bool isTextMeshProUGUI = component is TextMeshProUGUI;

        if (interruptionCount == 1)
        {
            randomTime = isTurningOn ? duration * 0.6f : duration * 0.4f;//Random.Range(duration * 0.4f, duration * 0.8f) : Random.Range(duration * 0.2f, duration * 0.6f);

            while (elapsedTime < randomTime)
            {
                float curveValue = isTurningOn ? colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime) : colorChangeCurveTurnOff.Evaluate(elapsedTime / randomTime);
                Color newColor = Color.Lerp(initialColor, targetColor, curveValue);

                if (isImage)
                {
                    (component as Image).color = newColor;
                }
                else if (isTextMeshProUGUI)
                {
                    (component as TextMeshProUGUI).color = newColor;
                }
                else
                {
                    (component as Renderer).material.color = newColor;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;

            initialColorSafer.a = 0.33f;
        }

        while (elapsedTime < (duration - randomTime))
        {
            float curveValue = isTurningOn ? colorChangeCurveTurnOn.Evaluate(elapsedTime / (duration - randomTime)) : colorChangeCurveTurnOff.Evaluate(elapsedTime / (duration - randomTime));
            Color newColor = Color.Lerp(initialColorSafer, targetColor, curveValue);

            if (isImage)
            {
                (component as Image).color = newColor;
            }
            else if (isTextMeshProUGUI)
            {
                (component as TextMeshProUGUI).color = newColor;
            }
            else
            {
                (component as Renderer).material.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isImage)
        {
        (component as Image).color = targetColor;
        }
        else if (isTextMeshProUGUI)
        {
            (component as TextMeshProUGUI).color = targetColor;
        }
        else
        {
            (component as Renderer).material.color = targetColor;
        }
        component.gameObject.SetActive(!isTurningOn);
    }
}