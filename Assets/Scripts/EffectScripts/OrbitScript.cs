using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    public bool isTurnOn = false;
    private bool isWaiting = false;
    [SerializeField] private GameObject torusPrefab; 
    [SerializeField] private GameObject mainCamera; 
    [SerializeField] private float smoothness = 0.1f; 
    [SerializeField] private float duration = 3f; 
    private int torusCount = 5; 
    private float minChangeIntervalTorus = 1f; 
    private float maxChangeIntervalTorus = 5f; 

    private GameObject[] torusArray; 
    private Vector3[] targetRotationAxes; 
    private Vector3[] currentRotationAxes; 
    private Vector3[] sizesTorus;
    private float[] speedsTorus;
    private Material[] materialsTorus;
    private float[] changeIntervalsTorus;
    private float[] timers;

    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public void SetOrbits(int count, Vector3[] sizes, float[] speeds, Material[] materials, float minChangeInterval, float maxChangeInterval)
    {
        if (sizes.Length != count || speeds.Length != count || materials.Length != count)
        {
            Debug.Log("Неправильно указаны массивы орбит");
        }

        torusCount = count;
        torusArray = new GameObject[torusCount];
        targetRotationAxes = new Vector3[torusCount];
        currentRotationAxes = new Vector3[torusCount];
        sizesTorus = sizes;
        speedsTorus = new float[torusCount];
        materialsTorus = materials;
        minChangeIntervalTorus = minChangeInterval;
        maxChangeIntervalTorus = maxChangeInterval;
        isWaiting = true;
        changeIntervalsTorus = new float[torusCount];
        timers = new float[torusCount];

        for (int i = 0; i < torusCount; i++)
        {
            speedsTorus[i] = speeds[i];
            torusArray[i] = Instantiate(torusPrefab, mainCamera.transform);
            torusArray[i].transform.position = new Vector3(0, 0, -50);
            torusArray[i].transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            torusArray[i].transform.localScale = sizesTorus[i];
            torusArray[i].GetComponent<Renderer>().material = materialsTorus[i];
            targetRotationAxes[i] = Random.onUnitSphere;
            currentRotationAxes[i] = targetRotationAxes[i];
            changeIntervalsTorus[i] = Random.Range(minChangeInterval, maxChangeInterval);



            timers[i] = 0f;
        }
        //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        StartCoroutine(SettingOrbits());
    }
    
    private IEnumerator SettingOrbits()
    {
        float elapsedTime = 0f;
        float movementTime = 3f;
        Vector3[] startPositions = new Vector3[torusArray.Length];
        Vector3[] startScales = new Vector3[torusArray.Length];

        for (int i = 0; i < torusArray.Length; i++)
        {
            startPositions[i] = torusArray[i].transform.position;

            torusArray[i].transform.localScale *= 100f;
            startScales[i] = torusArray[i].transform.localScale;
        }
        
        while (elapsedTime < movementTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / movementTime; // Нормализуем время от 0 до 1
            t = Mathf.SmoothStep(0, 1, t); // Добавляем плавность движения
            float curveValue = scaleCurve.Evaluate(t);
            for (int i = 0; i < torusArray.Length; i++)
            {
                if (torusArray[i] != null)
                {
                    torusArray[i].transform.localPosition = Vector3.Lerp(startPositions[i], new Vector3(0, 0, 9.3f), t);
                    torusArray[i].transform.localScale = Vector3.LerpUnclamped(startScales[i], sizesTorus[i], curveValue);
                }
            }

            yield return null;
        }
        
        // Убедимся, что все объекты точно дошли до цели
        for (int i = 0; i < torusArray.Length; i++)
        {
            if (torusArray[i] != null)
            {
                torusArray[i].transform.localPosition = new Vector3(0, 0, 9.3f);
                torusArray[i].transform.localScale = sizesTorus[i];
            }
        }

        for (int i = 0; i < torusArray.Length; i++)
        {
            if (torusArray[i] != null)
            {
                torusArray[i].transform.SetParent(transform);
                torusArray[i].transform.position = new Vector3(0, 0, 0f);
            }
        }
        isWaiting = false;

    }

    private void Update()
    {
        if (isWaiting) return;

        if (isTurnOn)
        {
            for (int i = 0; i < torusCount; i++)
            {
                currentRotationAxes[i] = Vector3.Lerp(currentRotationAxes[i], targetRotationAxes[i], smoothness * Time.deltaTime);
                torusArray[i].transform.Rotate(currentRotationAxes[i] * speedsTorus[i] * Time.deltaTime);
                timers[i] += Time.deltaTime;

                if (timers[i] >= changeIntervalsTorus[i])
                {
                    ChangeRotation(i);
                    timers[i] = 0f;
                }
            }
        }
        else if (torusArray != null && torusArray.Length > 0)
        {
            for (int i = 0; i < torusCount; i++)
            {
                Destroy(torusArray[i]);

            }
        }
    }

    private void ChangeRotation(int index)
    {
        targetRotationAxes[index] = Random.onUnitSphere;

        changeIntervalsTorus[index] = Random.Range(minChangeIntervalTorus, maxChangeIntervalTorus);
    }
}