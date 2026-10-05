using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuCreditsScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private MenuCreditsAnimationManagerScript animationManager;
    [SerializeField] private MenuLogoNeonFlinkeringScript MLNFS;
    [SerializeField] private GameObject[] parentObjects;

    [Header("Settings")]
    [SerializeField] private float timeForLine;
    [SerializeField] private float speedCamera;
    [FormerlySerializedAs("duration")]
    [SerializeField] private float cameraSpeedUpDuration = 1.5f;
    [SerializeField] private float durationCameraReturn = 1.5f;
    [SerializeField] private float currentSpeed = 0f;

    [Header("Logo")]
    [SerializeField] private float logoTurnOffDuration = 0.75f;
    [SerializeField] private float logoTurnOnDuration = 0.75f;

    [Header("Word delays")]
    [SerializeField] private float delayMinFirstLineNextWord = 0.7f;
    [SerializeField] private float delayMaxFirstLineNextWord = 1.5f;

    [SerializeField] private float delayMinOtherLinesNextWord = 0.35f;
    [SerializeField] private float delayMaxOtherLinesNextWord = 0.5f;

    [SerializeField] private float durationModifierFinalWord= 0.5f;

    [Header("Line time modifiers")]
    [SerializeField] private CreditsLineTimeModifierScript[] lineTimeModifiers;

    private GameObject[][] sortedChildren;
    private Vector3 camPos;
    private float t = 0f;
    private bool isStarted = false;
    private bool isEnded = false;

    private void Start()
    {
        camPos = cam.transform.position;
        sortedChildren = new GameObject[parentObjects.Length][];

        for (int i = 0; i < parentObjects.Length; i++)
            if (parentObjects[i] != null)
                sortedChildren[i] = parentObjects[i].transform
                    .Cast<Transform>()
                    .OrderBy(t => t.position.x)
                    .Select(t => t.gameObject)
                    .ToArray();

        for (int i = 0; i < sortedChildren.Length; i++)
            foreach (var child in sortedChildren[i])
                animationManager.HideWord(child);
    }

    private void Update()
    {
        if (isStarted && !isEnded)
        {
            if (t < cameraSpeedUpDuration)
            {
                t += Time.deltaTime;
                currentSpeed = Mathf.Lerp(0, speedCamera, t / cameraSpeedUpDuration);
            }
            else currentSpeed = speedCamera;

            animationManager.MoveCameraDown(cam.transform, currentSpeed * Time.deltaTime);
        }
        else if (isEnded && currentSpeed > 0)
        {
            t -= Time.deltaTime;
            currentSpeed = Mathf.Lerp(0, speedCamera, t / cameraSpeedUpDuration);
            animationManager.MoveCameraDown(cam.transform, currentSpeed * Time.deltaTime);
        }
    }

    public void StartCredits()
    {
        StartCoroutine(SettingMaterial());
    }

    public void EndCredits()
    {
        StopAllCoroutines();
        isStarted = false;
        isEnded = true;
        StartCoroutine(ReturningCamera());
        StartCoroutine(TurningOffWords());
    }

    private IEnumerator ReturningCamera()
    {
        t = 0f;
        yield return StartCoroutine(animationManager.ReturnCameraAsync(cam.transform, camPos, durationCameraReturn));
    }

    private IEnumerator TurningOffWords()
    {
        if (!MLNFS.isTurnOn) MLNFS.LogoTurningOnAndOff(logoTurnOnDuration, true, true, true, false);

        yield return StartCoroutine(animationManager.TurnOffWordsAsync(sortedChildren, timeForLine));
    }

    private IEnumerator SettingMaterial()
    {
        MLNFS.LogoTurningOnAndOff(logoTurnOffDuration, false, true, false, false);

        yield return new WaitForSeconds(timeForLine);
        for (int i = 0; i < sortedChildren.Length; i++)
        {
            if (i == 1)
            {
                MLNFS.LogoTurningOnAndOff(timeForLine, true, true, true, false);

                yield return new WaitForSeconds(timeForLine);
                isStarted = true;
                isEnded = false;
            }
            else foreach (var modifier in lineTimeModifiers)
                    if (modifier.lineIndex == i) timeForLine *= modifier.timeModifier;

            float timeForWord = timeForLine / sortedChildren[i].Length;
            
            for (int j = 0; j < sortedChildren[i].Length; j++)
            {
                animationManager.ShowWord(sortedChildren[i][j]);

                if (j < sortedChildren[i].Length - 1)
                {
                    float nextWordDelayMin = (i == 0 ? delayMinFirstLineNextWord : delayMinOtherLinesNextWord) * timeForWord;
                    float nextWordDelayMax = (i == 0 ? delayMaxFirstLineNextWord : delayMaxOtherLinesNextWord) * timeForWord;
                    yield return new WaitForSeconds(UnityEngine.Random.Range(nextWordDelayMin, nextWordDelayMax));

                    animationManager.ShowWord(sortedChildren[i][j + 1]);
                }

                TextMeshPro textMesh = sortedChildren[i][j].GetComponent<TextMeshPro>();

                yield return StartCoroutine(animationManager.ChangingColorSmoothly(textMesh, timeForWord, Color.gray, Color.white));

                if (i == sortedChildren.Length - 1 && j == sortedChildren[i].Length - 1)
                {
                    yield return new WaitForSeconds(timeForLine * durationModifierFinalWord);
                    isEnded = true;
                }
            }
        }
    }
}

[Serializable]
public class CreditsLineTimeModifierScript
{
    public int lineIndex;
    public float timeModifier;
}
