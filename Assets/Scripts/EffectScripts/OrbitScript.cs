using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    [SerializeField] private GameObject torus;
    [SerializeField] private GameObject torus1;
    public float rotationSpeed = 100f; // Скорость вращения
    public float changeInterval = 2f; // Интервал смены направления
    public float smoothness = 0.1f; // Плавность смены направления

    private Vector3 targetRotationAxis;
    private Vector3 currentRotationAxis;
    private float timer;

    void Start()
    {
        ChangeRotation();
        currentRotationAxis = targetRotationAxis;
    }

    void Update()
    {
        currentRotationAxis = Vector3.Lerp(currentRotationAxis, targetRotationAxis, smoothness * Time.deltaTime);
        torus.transform.Rotate(currentRotationAxis * rotationSpeed * Time.deltaTime);
        torus1.transform.Rotate(currentRotationAxis * rotationSpeed * Time.deltaTime* -1);

        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            ChangeRotation();
            timer = 0f;
        }
    }

    void ChangeRotation()
    {
        targetRotationAxis = Random.onUnitSphere; // Генерируем случайную ось вращения
    }
}