using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuCreditsScript : MonoBehaviour
{
    private const int LogoLineIndex = 1;

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private MenuCreditsAnimationManagerScript animationManager;
    [SerializeField] private MenuLogoNeonFlinkeringScript menuLogo;
    [SerializeField] private GameObject[] parentObjects;
    [SerializeField] private CreditsSettings settings;

    private GameObject[][] sortedChildren;
    private Transform camTransform;
    private Vector3 camPos;
    private float t = 0f;
    private float currentSpeed = 0f;
    private CreditsState state = CreditsState.Idle;

    private void Start()
    {
        if (cam == null || animationManager == null || menuLogo == null || settings == null)
            Debug.LogError("references are not assigned");

        camTransform = cam.transform;
        camPos = camTransform.position;
        sortedChildren = new GameObject[parentObjects.Length][];

        for (int i = 0; i < parentObjects.Length; i++)
        {
            if (parentObjects[i] == null) continue;

            sortedChildren[i] = parentObjects[i].transform
                .Cast<Transform>()
                .OrderBy(t => t.position.x)
                .Select(t => t.gameObject)
                .ToArray();

            foreach (var child in sortedChildren[i]) animationManager.HideWord(child);
        }
    }

    private void Update()
    {
        if (state == CreditsState.Running)
        {
            if (t < settings.durationCameraSpeedUp)
            {
                t += Time.deltaTime;
                currentSpeed = Mathf.Lerp(0, settings.cameraSpeed, t / settings.durationCameraSpeedUp);
            }
            else currentSpeed = settings.cameraSpeed;

            animationManager.MoveCameraDown(camTransform, currentSpeed * Time.deltaTime);
        }
        else if (state == CreditsState.Decelerating)
        {
            t -= Time.deltaTime;
            if (t <= 0f)
            {
                t = 0f;
                currentSpeed = 0f;
                state = CreditsState.Idle;
            }
            else
            {
                currentSpeed = Mathf.Lerp(0, settings.cameraSpeed, t / settings.durationCameraSpeedUp);
                animationManager.MoveCameraDown(camTransform, currentSpeed * Time.deltaTime);
            }
        }
    }

    public void StartCredits()
    {
        state = CreditsState.Starting;
        StartCoroutine(PlayCreditsSequence());
    }

    public void EndCredits()
    {
        StopAllCoroutines();

        animationManager.StopAllCoroutines();
        state = CreditsState.Ending;
        currentSpeed = 0f;
        StartCoroutine(ReturningCamera());
        StartCoroutine(TurningOffWords());
    }

    private IEnumerator ReturningCamera()
    {
        yield return StartCoroutine(animationManager.ReturnCameraAsync(camTransform, camPos, settings.durationCameraReturn));
    }

    private IEnumerator TurningOffWords()
    {
        if (!menuLogo.isTurnOn) menuLogo.LogoTurningOnAndOff(settings.durationLogoTurnOn, true, true, true, false);

        yield return StartCoroutine(animationManager.TurnOffWordsAsync(sortedChildren, settings.durationLine));
    }

    private IEnumerator PlayCreditsSequence()
    {
        menuLogo.LogoTurningOnAndOff(settings.durationLogoTurnOff, false, true, false, false);

        yield return new WaitForSeconds(settings.durationLine);

        float lineTime = settings.durationLine;

        for (int i = 0; i < sortedChildren.Length; i++)
        {
            if (sortedChildren[i] == null || sortedChildren[i].Length == 0) continue;

            if (i == LogoLineIndex)
            {
                menuLogo.LogoTurningOnAndOff(settings.durationLine, true, true, true, false);
                yield return new WaitForSeconds(settings.durationLine);
                t = 0f;
                currentSpeed = 0f;
                state = CreditsState.Running;
            }
            else if (settings.lineTimeModifiers != null)
            {
                foreach (var modifier in settings.lineTimeModifiers)
                    if (modifier != null && modifier.lineIndex == i)
                        lineTime *= modifier.timeModifier;
            }

            float timeForWord = lineTime / sortedChildren[i].Length;

            animationManager.ShowWord(sortedChildren[i][0]);

            for (int j = 0; j < sortedChildren[i].Length; j++)
            {
                if (j < sortedChildren[i].Length - 1)
                {
                    float nextWordDelayMin = (i == 0 ? settings.delayFirstLineNextWordMin : settings.delayOtherLinesNextWordMin) * timeForWord;
                    float nextWordDelayMax = (i == 0 ? settings.delayFirstLineNextWordMax : settings.delayOtherLinesNextWordMax) * timeForWord;
                    yield return new WaitForSeconds(UnityEngine.Random.Range(nextWordDelayMin, nextWordDelayMax));

                    animationManager.ShowWord(sortedChildren[i][j + 1]);
                }

                if (!sortedChildren[i][j].TryGetComponent<TextMeshPro>(out var textMesh))
                {
                    Debug.LogWarning($"{nameof(MenuCreditsScript)}: word '{sortedChildren[i][j].name}' has no TextMeshPro.", sortedChildren[i][j]);
                    continue;
                }

                yield return StartCoroutine(animationManager.ChangingColorSmoothly(textMesh, timeForWord, Color.gray, Color.white));

                if (i == sortedChildren.Length - 1 && j == sortedChildren[i].Length - 1)
                {
                    yield return new WaitForSeconds(lineTime * settings.durationModifierFinalWord);
                    state = CreditsState.Decelerating;
                }
            }
        }
    }
}

[Serializable]
public class CreditsLineTimeModifier
{
    public int lineIndex;
    public float timeModifier;
}
