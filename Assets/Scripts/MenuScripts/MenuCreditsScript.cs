using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCreditsScript : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private Vector3 camPos;
    [SerializeField] private MenuLogoNeonFlinkeringScript MLNFS;
    [SerializeField] private GameObject[] parentObjects;
    [SerializeField] private float timeForLine;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float t = 0f; // Время интерполяции
    [SerializeField] private float duration = 1.5f; // Длительность ускорения/замедления
    [SerializeField] private float durationCameraReturn = 1.5f; // Длительность ускорения/замедления
    [SerializeField] private float currentSpeed = 0f; // Текущая скорость
    private GameObject[][] sortedChildren;
    public AnimationCurve colorChangeCurveTurnOn;
    public Image wall;
    public bool isStarted = false;
    public bool isEnded = false;

    private void Start()
    {
        camPos = cam.transform.position;
        sortedChildren = new GameObject[parentObjects.Length][];

        for (int i = 0; i < parentObjects.Length; i++)
        {
            if (parentObjects[i] != null)
            {
                sortedChildren[i] = parentObjects[i].transform
                    .Cast<Transform>()
                    .OrderBy(t => t.position.x)
                    .Select(t => t.gameObject)
                    .ToArray();
            }
        }

        for (int i = 0; i < sortedChildren.Length; i++)
        {
            foreach (var child in sortedChildren[i])
            {
                TextMeshPro textMesh = child.GetComponent<TextMeshPro>();

                if (textMesh != null)
                {
                    textMesh.color = Color.gray;
                }
                child.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (isStarted && !isEnded)
        {
            if (t < duration)
            {
                t += Time.deltaTime;
                currentSpeed = Mathf.Lerp(0, cameraSpeed, t / duration);
            }
            else
            {
                currentSpeed = cameraSpeed;
            }

            cam.transform.position += Vector3.down * currentSpeed * Time.deltaTime;
        }
        else if (isEnded && currentSpeed > 0)
        {
            t -= Time.deltaTime;
            currentSpeed = Mathf.Lerp(0, cameraSpeed, t / duration);
            cam.transform.position += Vector3.down * currentSpeed * Time.deltaTime;
        }
    }

    public void StartCredits()
    {
        StartCoroutine(SettingMaterial());
    }

    public void EndCredits()
    {
        wall.gameObject.SetActive(true);
        StopAllCoroutines();
        isStarted = false;
        isEnded = true;
        StartCoroutine(ReturningCamera());
        
        StartCoroutine(TurningOffWords());
    }

    private IEnumerator ReturningCamera()
    {
        Vector3 startPosition = cam.transform.position;
        Vector3 targetPosition = camPos;
        float elapsedTime = 0f;
         t = 0f; // Время интерполяции
        while (elapsedTime < durationCameraReturn)
        {
            wall.gameObject.SetActive(true);
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / durationCameraReturn);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPosition;
        wall.gameObject.SetActive(false);
    }

    private IEnumerator TurningOffWords()
    {
        if (!MLNFS.isTurnOn)
        {
            MLNFS.LogoTurningOnAndOff(0.75f, true, true, true, false);
            ///MLNFS.LogoTurningOnAndOff( Color.gray, Color.white, 0.75f, 2f, 3f, true, true);
        }
        
        for (int i = 0; i < sortedChildren.Length; i++)
        {
            foreach (var child in sortedChildren[i])
            {
                TextMeshPro textMesh = child.GetComponent<TextMeshPro>();

                if (child.activeSelf == true)
                {
                    StartCoroutine(ChangingColorSmoothly(textMesh, timeForLine / 4, Color.white, Color.clear));
                }
            }
        }
        yield return new WaitForSeconds(timeForLine);

        for (int i = 0; i < sortedChildren.Length; i++)
        {
            foreach (var child in sortedChildren[i])
            {
                child.SetActive(false);
            }
        }


    }


    private IEnumerator SettingMaterial()
    {
        MLNFS.LogoTurningOnAndOff(0.75f, false, true, false, false);

        yield return new WaitForSeconds(timeForLine);
        for (int i = 0; i < sortedChildren.Length; i++)
        {
            if (i == 1)
            {
                MLNFS.LogoTurningOnAndOff(timeForLine, true, true, true, false);
                
                yield return new WaitForSeconds(timeForLine);
                isStarted = true;
                isEnded = false;

            }else if (i == 13)
            {
                timeForLine *= 1.25f;
            }
            else if (i == 20)
            {
                timeForLine *= 1.5f;
            }
            float timeForWord = timeForLine / sortedChildren[i].Length;
            for (int j = 0; j < sortedChildren[i].Length; j++)
            {
                TextMeshPro textMesh = sortedChildren[i][j].GetComponent<TextMeshPro>();

                if (textMesh != null)
                {
                    textMesh.color = Color.gray;
                }
                sortedChildren[i][j].SetActive(true);

                

                if (j < sortedChildren[i].Length - 1)
                {
                    yield return new WaitForSeconds(Random.Range((i == 0 ? 2f : 1f) * 0.7f * timeForWord / 2,(i == 0 ? 3f : 1f) * timeForWord / 2));

                    TextMeshPro textMeshNext = sortedChildren[i][j+1].GetComponent<TextMeshPro>();

                    if (textMeshNext != null)
                    {
                        textMeshNext.color = Color.gray;
                    }
                    sortedChildren[i][j+1].SetActive(true);
                }

                yield return StartCoroutine(ChangingColorSmoothly(textMesh, timeForWord, Color.gray, Color.white));

                if (i == sortedChildren.Length - 1 && j == sortedChildren[i].Length - 1)
                {
                    yield return new WaitForSeconds(timeForLine/2);
                    isEnded = true;
                }
            }
        }
    }

    private IEnumerator ChangingColorSmoothly(TextMeshPro text, float time, Color initialColor, Color targetColor)
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
            
        
        //Debug.Log(interruptionCount);
        Color initialColorSafer = initialColor;
        if (interruptionCount == 0)
        {
            while (elapsedTime < time )
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
                float curveValue =  colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime);
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

            randomTime = Random.Range(time * 0.22f, time * 0.45f) ;
            randomTimeExtra = Random.Range((time - randomTime)*1.0005f, time * 0.9f);
            while (elapsedTime < randomTime)
            {
                float curveValue = colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime) ;
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

            while (elapsedTime < (time -  randomTimeExtra))
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