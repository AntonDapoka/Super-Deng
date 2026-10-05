using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCreditsAnimationManagerScript : MonoBehaviour
{
    [SerializeField] private AnimationCurve colorChangeCurveTurnOn;
    [SerializeField] private AnimationCurve speedByDistanceToTargetCurve;
    [SerializeField] private float cameraArrivalThreshold = 0.001f;
    [SerializeField] private float durationModifierWordsFadeOut = 0.25f;

    [Header("Glitch chances (roll of 0..glitchRollMax)")]
    [SerializeField] private int glitchRollMax = 100;
    [SerializeField] private int singleInterruptionMinRoll = 40;
    [SerializeField] private int doubleInterruptionMinRoll = 87;

    [Header("Glitch timing")]
    [SerializeField] private float singleInterruptionMinTimeShare = 0.2f;
    [SerializeField] private float singleInterruptionMaxTimeShare = 0.8f;
    [SerializeField] private float doubleInterruptionFirstStopMinShare = 0.22f;
    [SerializeField] private float doubleInterruptionFirstStopMaxShare = 0.45f;
    [SerializeField] private float doubleInterruptionSecondStopMinExtra = 1.0005f;
    [SerializeField] private float doubleInterruptionSecondStopMaxShare = 0.9f;

    private const float MinSegmentDuration = 0.001f;

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

        if (duration <= 0f || startDistance <= cameraArrivalThreshold)
        {
            camera.position = targetPosition;
            yield break;
        }

        float baseSpeed = startDistance / duration;

        while (Vector3.Distance(camera.position, targetPosition) > cameraArrivalThreshold)
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
        float fadeDuration = timeForLine * durationModifierWordsFadeOut;

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
                float curveValue = colorChangeCurveTurnOn.Evaluate(Mathf.Clamp01(elapsedTime / segmentDuration));
                text.color = Color.Lerp(initialColor, targetColor, curveValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        text.color = targetColor;
    }


    private List<float> BuildGlitchSegmentDurations(float totalTime)
    {
        int roll = Random.Range(0, glitchRollMax);

        int interruptionCount = 0;
        if (roll >= singleInterruptionMinRoll && roll <= doubleInterruptionMinRoll) interruptionCount = 1;
        else if (roll > doubleInterruptionMinRoll) interruptionCount = 2;

        var boundaries = new List<float>(2);
        if (interruptionCount == 1)
        {
            boundaries.Add(Random.Range(totalTime * singleInterruptionMinTimeShare, totalTime * singleInterruptionMaxTimeShare));
        }
        else if (interruptionCount == 2)
        {
            float firstBoundary = Random.Range(totalTime * doubleInterruptionFirstStopMinShare, totalTime * doubleInterruptionFirstStopMaxShare);
            float secondBoundary = Random.Range((totalTime - firstBoundary) * doubleInterruptionSecondStopMinExtra, totalTime * doubleInterruptionSecondStopMaxShare);
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

        if (totalTime - previousBoundary >= MinSegmentDuration)
            segmentDurations.Add(totalTime - previousBoundary);

        if (segmentDurations.Count == 0) segmentDurations.Add(totalTime);

        return segmentDurations;
    }
}
