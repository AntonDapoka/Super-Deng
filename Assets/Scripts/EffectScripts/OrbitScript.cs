using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    [SerializeField] private GameObject torusPrefab; // Префаб тора
    [SerializeField] private int torusCount = 5; // Количество торов
    public float rotationSpeed = 100f; // Скорость вращения
    public float minChangeInterval = 1f; // Минимальный интервал смены направления
    public float maxChangeInterval = 5f; // Максимальный интервал смены направления
    public float smoothness = 0.1f; // Плавность смены направления

    private GameObject[] torusArray; // Массив торов
    private Vector3[] targetRotationAxes; // Массив целевых осей вращения
    private Vector3[] currentRotationAxes; // Массив текущих осей вращения
    [SerializeField]  private Material[] materials;
    private float[] changeIntervals; // Массив уникальных интервалов для каждого тора
    private float[] timers; // Массив индивидуальных таймеров для каждого тора

    void Start()
    {
        // Инициализация массивов
        torusArray = new GameObject[torusCount];
        targetRotationAxes = new Vector3[torusCount];
        currentRotationAxes = new Vector3[torusCount];
        changeIntervals = new float[torusCount];
        timers = new float[torusCount];

        for (int i = 0; i < torusCount; i++)
        {
            // Создание тора и добавление его в массив
            torusArray[i] = Instantiate(torusPrefab, transform);

            // Инициализация осей вращения
            targetRotationAxes[i] = Random.onUnitSphere;
            currentRotationAxes[i] = targetRotationAxes[i];

            // Генерация уникального интервала для каждого тора
            changeIntervals[i] = Random.Range(minChangeInterval, maxChangeInterval);

            // Инициализация таймера
            timers[i] = 0f;
        }
    }

    void Update()
    {
        for (int i = 0; i < torusCount; i++)
        {
            // Плавное изменение оси вращения
            currentRotationAxes[i] = Vector3.Lerp(currentRotationAxes[i], targetRotationAxes[i], smoothness * Time.deltaTime);
            torusArray[i].transform.Rotate(currentRotationAxes[i] * rotationSpeed * Time.deltaTime);

            // Обновление таймера
            timers[i] += Time.deltaTime;

            // Если таймер превысил интервал, меняем ось вращения
            if (timers[i] >= changeIntervals[i])
            {
                ChangeRotation(i);
                timers[i] = 0f;
            }
        }
    }

    void ChangeRotation(int index)
    {
        // Генерация новой случайной оси вращения для конкретного тора
        targetRotationAxes[index] = Random.onUnitSphere;

        // Опционально: можно обновить интервал для этого тора
        changeIntervals[index] = Random.Range(minChangeInterval, maxChangeInterval);
    }
}