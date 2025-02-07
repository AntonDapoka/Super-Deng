using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    public bool isTurnOn = false;
    [SerializeField] private GameObject torusPrefab; // Префаб тора
    [SerializeField] private float smoothness = 0.1f; // Плавность смены направления
    [SerializeField] private float duration = 3f; 
    private int torusCount = 5; // Количество торов
    private float minChangeIntervalTorus = 1f; // Минимальный интервал смены направления
    private float maxChangeIntervalTorus = 5f; // Максимальный интервал смены направления

    private GameObject[] torusArray; // Массив торов
    private Vector3[] targetRotationAxes; // Массив целевых осей вращения
    private Vector3[] currentRotationAxes; // Массив текущих осей вращения
    private Vector3[] sizesTorus;
    private float[] speedsTorus;
    private Material[] materialsTorus;
    private float[] changeIntervalsTorus; // Массив уникальных интервалов для каждого тора
    private float[] timers; // Массив индивидуальных таймеров для каждого тора

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
        speedsTorus = speeds;   
        materialsTorus = materials;
        minChangeIntervalTorus = minChangeInterval;
        maxChangeIntervalTorus = maxChangeInterval;

        changeIntervalsTorus = new float[torusCount];
        timers = new float[torusCount];

        for (int i = 0; i < torusCount; i++)
        {
            torusArray[i] = Instantiate(torusPrefab, transform);
            torusArray[i].transform.localScale = sizesTorus[i];
            torusArray[i].GetComponent<Renderer>().material = materialsTorus[i];
            targetRotationAxes[i] = Random.onUnitSphere;
            currentRotationAxes[i] = targetRotationAxes[i];


            changeIntervalsTorus[i] = Random.Range(minChangeInterval, maxChangeInterval);

            timers[i] = 0f;
        }

        StartCoroutine(SettingOrbits());
    }

    private IEnumerator SettingOrbits()
    {
        for (int i = 0; i < torusArray.Length; i++)
        {
            if (torusArray[i] != null)
            {
                torusArray[i].transform.localScale *= 100f;
                speedsTorus[i] /= 10f;
            }
        }


        Vector3[] startScales = new Vector3[torusArray.Length];
        for (int i = 0; i < torusArray.Length; i++)
        {
            if (torusArray[i] != null)
            {
                startScales[i] = torusArray[i].transform.localScale;

            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; 
            float curveValue = scaleCurve.Evaluate(t); // Получаем значение из кривой

            for (int i = 0; i < torusArray.Length; i++)
            {
                if (torusArray[i] != null)
                {
                    torusArray[i].transform.localScale = Vector3.LerpUnclamped(startScales[i], sizesTorus[i], curveValue);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < torusArray.Length; i++)
        {
            if (torusArray[i] != null)
            {
                torusArray[i].transform.localScale = sizesTorus[i];
                speedsTorus[i] *= 10f;
            }
        }
    }

    private void Update()
    {
        if (isTurnOn)
        {
            for (int i = 0; i < torusCount; i++)
            {
                // Плавное изменение оси вращения
                currentRotationAxes[i] = Vector3.Lerp(currentRotationAxes[i], targetRotationAxes[i], smoothness * Time.deltaTime);
                torusArray[i].transform.Rotate(currentRotationAxes[i] * speedsTorus[i] * Time.deltaTime);

                // Обновление таймера
                timers[i] += Time.deltaTime;

                // Если таймер превысил интервал, меняем ось вращения
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
        // Генерация новой случайной оси вращения для конкретного тора
        targetRotationAxes[index] = Random.onUnitSphere;

        // Опционально: можно обновить интервал для этого тора
        changeIntervalsTorus[index] = Random.Range(minChangeIntervalTorus, maxChangeIntervalTorus);
    }
}