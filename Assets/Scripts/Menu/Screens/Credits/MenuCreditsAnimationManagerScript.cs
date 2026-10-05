using System.Collections;
using TMPro;
using UnityEngine;

public class MenuCreditsAnimationManagerScript : MonoBehaviour
{
    [SerializeField] private AnimationCurve colorChangeCurveTurnOn;
    [SerializeField] private AnimationCurve speedByDistanceToTargetCurve;

    public void ShowWord(GameObject word)
    {
        if (word.TryGetComponent<TextMeshPro>(out var textMesh)) textMesh.color = Color.gray;
        word.SetActive(true);
    }

    public void HideWord(GameObject word)
    {
        if (word.TryGetComponent<TextMeshPro>(out var textMesh)) textMesh.color = Color.gray;
        word.SetActive(false);
    }

    public void MoveCameraDown(Transform camera, float distance)
    {
        camera.position += Vector3.down * distance;
    }

    public IEnumerator ReturnCameraAsync(Transform camera, Vector3 targetPosition, float duration)
    {
        float startDistance = Vector3.Distance(camera.position, targetPosition);
        float baseSpeed = startDistance / duration;

        while (Vector3.Distance(camera.position, targetPosition) > 0.001f)
        {
            float distanceLeft = Vector3.Distance(camera.position, targetPosition);
            float speed = baseSpeed * speedByDistanceToTargetCurve.Evaluate(Mathf.Clamp01(distanceLeft / startDistance));
            camera.position = Vector3.MoveTowards(camera.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        camera.position = targetPosition;
    }

    public IEnumerator TurnOffWordsAsync(GameObject[][] sortedChildren, float timeForLine)
    {
        foreach (var line in sortedChildren)
            foreach (var child in line)
                if (child.activeSelf && child.TryGetComponent<TextMeshPro>(out var textMesh))
                    StartCoroutine(ChangingColorSmoothly(textMesh, timeForLine / 4, Color.white, Color.clear));

        yield return new WaitForSeconds(timeForLine);

        foreach (var line in sortedChildren)
            foreach (var child in line)
                child.SetActive(false);
    }

    public IEnumerator ChangingColorSmoothly(TextMeshPro text, float time, Color initialColor, Color targetColor)
    {
        float elapsedTime = 0f;
        float randomTime = 0f;
        float randomTimeExtra = 0f;
        int randomNum = Random.Range(0, 100);
        int interruptionCount = 0;
        
        if (randomNum >= 40 && randomNum <= 87)
        {
            interruptionCount = 1;
        }
        else if (randomNum > 87)
        {
            interruptionCount = 2;
        }

        Color initialColorSafer = initialColor;
        if (interruptionCount == 0)
        {
            while (elapsedTime < time)
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / (time - randomTime - randomTimeExtra));
                Color newColor = Color.Lerp(initialColorSafer, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            text.color = targetColor;
        }
        else if (interruptionCount == 1)
        {
            randomTime = Random.Range(time * 0.2f, time * 0.8f);

            while (elapsedTime < randomTime)
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime);
                Color newColor = Color.Lerp(initialColor, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;

            while (elapsedTime < (time - randomTime))
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / (time - randomTime - randomTimeExtra));
                Color newColor = Color.Lerp(initialColorSafer, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            text.color = targetColor;
        }
        else if (interruptionCount == 2)
        {
            randomTime = Random.Range(time * 0.22f, time * 0.45f);
            randomTimeExtra = Random.Range((time - randomTime) * 1.0005f, time * 0.9f);
            while (elapsedTime < randomTime)
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime);
                Color newColor = Color.Lerp(initialColor, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;

            while (elapsedTime < randomTimeExtra - randomTime)
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / (randomTimeExtra - randomTime));
                Color newColor = Color.Lerp(initialColor, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;

            while (elapsedTime < (time - randomTimeExtra))
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / (time - randomTimeExtra));
                Color newColor = Color.Lerp(initialColorSafer, targetColor, curveValue);
                text.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            text.color = targetColor;
        }
    }
}
