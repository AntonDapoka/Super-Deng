using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCreditsAnimationManagerScript : MonoBehaviour
{
    [SerializeField] private CreditsSettings settings;
    private const float MinSegmentDuration = 0.001f;

    private void Start()
    {
        if (settings == null) Debug.LogError("settings are not assigned");
    }

    public void ShowWord(GameObject word)
    {
        SetWordGray(word);
        word.SetActive(true);
    }

    public void HideWord(GameObject word)
    {
        SetWordGray(word);
        word.SetActive(false);
    }

    private static void SetWordGray(GameObject word)
    {
        if (word.TryGetComponent<TextMeshPro>(out var textMesh)) textMesh.color = Color.gray;
    }

    public void MoveCameraDown(Transform camera, float distance)
    {
        camera.position += Vector3.down * distance;
    }

    public IEnumerator ReturnCameraAsync(Transform camera, Vector3 targetPosition, float duration)
    {
        float startDistance = Vector3.Distance(camera.position, targetPosition);

        if (duration <= 0f || startDistance <= settings.thresholdCameraArrival)
        {
            camera.position = targetPosition;
            yield break;
        }

        float baseSpeed = startDistance / duration;

        while (Vector3.Distance(camera.position, targetPosition) > settings.thresholdCameraArrival)
        {
            float distanceLeft = Vector3.Distance(camera.position, targetPosition);
            float speed = baseSpeed * settings.curveSpeedByDistanceToTarget.Evaluate(Mathf.Clamp01(distanceLeft / startDistance));
            camera.position = Vector3.MoveTowards(camera.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        camera.position = targetPosition;
    }

    public IEnumerator TurnOffWordsAsync(GameObject[][] sortedChildren, float timeForLine)
    {
        float fadeDuration = timeForLine * settings.durationModifierWordsFadeOut;

        foreach (var line in sortedChildren)
        {
            if (line == null) continue;

            foreach (var child in line)
                if (child != null && child.activeSelf && child.TryGetComponent<TextMeshPro>(out var textMesh))
                    StartCoroutine(ChangingColorSmoothly(textMesh, fadeDuration, Color.white, Color.clear));
        }

        yield return new WaitForSeconds(timeForLine);

        foreach (var line in sortedChildren)
        {
            if (line == null) continue;

            foreach (var child in line)
                if (child != null) child.SetActive(false);
        }
    }

    public IEnumerator ChangingColorSmoothly(TextMeshPro text, float time, Color initialColor, Color targetColor)
    {
        if (time <= 0f)
        {
            text.color = targetColor;
            yield break;
        }

        foreach (float segmentDuration in BuildGlitchSegmentDurations(time))
        {
            float elapsedTime = 0f;
            while (elapsedTime < segmentDuration)
            {
                float curveValue = settings.curveColorChangeTurnOn.Evaluate(Mathf.Clamp01(elapsedTime / segmentDuration));
                text.color = Color.Lerp(initialColor, targetColor, curveValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        text.color = targetColor;
    }

    private List<float> BuildGlitchSegmentDurations(float totalTime)
    {
        int roll = Random.Range(0, settings.maxGlitchRoll);

        int interruptionCount = 0;
        if (roll >= settings.rollSingleInterruptionMin && roll <= settings.rollDoubleInterruptionMin) interruptionCount = 1;
        else if (roll > settings.rollDoubleInterruptionMin) interruptionCount = 2;

        var boundaries = new List<float>(2);
        if (interruptionCount == 1)
        {
            boundaries.Add(Random.Range(totalTime * settings.shareTimeSingleInterruptionMin, totalTime * settings.shareTimeSingleInterruptionMax));
        }
        else if (interruptionCount == 2)
        {
            float firstBoundary = Random.Range(totalTime * settings.shareTimeDoubleInterruptionFirstStopMin, totalTime * settings.shareTimeDoubleInterruptionFirstStopMax);
            float secondBoundary = Random.Range((totalTime - firstBoundary) * settings.shareTimeDoubleInterruptionSecondStopExtraMin, totalTime * settings.shareTimeDoubleInterruptionSecondStopMax);
            boundaries.Add(firstBoundary);
            boundaries.Add(secondBoundary);
        }

        var segmentDurations = new List<float>(boundaries.Count + 1);
        float previousBoundary = 0f;
        foreach (float boundary in boundaries)
        {
            float clampedBoundary = Mathf.Clamp(boundary, 0f, totalTime);
            if (clampedBoundary - previousBoundary < MinSegmentDuration) continue;

            segmentDurations.Add(clampedBoundary - previousBoundary);
            previousBoundary = clampedBoundary;
        }

        if (totalTime - previousBoundary >= MinSegmentDuration) segmentDurations.Add(totalTime - previousBoundary);
        if (segmentDurations.Count == 0) segmentDurations.Add(totalTime);

        return segmentDurations;
    }
}
