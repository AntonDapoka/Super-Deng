using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    [SerializeField] private GameObject torusPrefab; // ������ ����
    [SerializeField] private int torusCount = 5; // ���������� �����
    public float rotationSpeed = 100f; // �������� ��������
    public float minChangeInterval = 1f; // ����������� �������� ����� �����������
    public float maxChangeInterval = 5f; // ������������ �������� ����� �����������
    public float smoothness = 0.1f; // ��������� ����� �����������

    private GameObject[] torusArray; // ������ �����
    private Vector3[] targetRotationAxes; // ������ ������� ���� ��������
    private Vector3[] currentRotationAxes; // ������ ������� ���� ��������
    [SerializeField]  private Material[] materials;
    private float[] changeIntervals; // ������ ���������� ���������� ��� ������� ����
    private float[] timers; // ������ �������������� �������� ��� ������� ����

    void Start()
    {
        // ������������� ��������
        torusArray = new GameObject[torusCount];
        targetRotationAxes = new Vector3[torusCount];
        currentRotationAxes = new Vector3[torusCount];
        changeIntervals = new float[torusCount];
        timers = new float[torusCount];

        for (int i = 0; i < torusCount; i++)
        {
            // �������� ���� � ���������� ��� � ������
            torusArray[i] = Instantiate(torusPrefab, transform);

            // ������������� ���� ��������
            targetRotationAxes[i] = Random.onUnitSphere;
            currentRotationAxes[i] = targetRotationAxes[i];

            // ��������� ����������� ��������� ��� ������� ����
            changeIntervals[i] = Random.Range(minChangeInterval, maxChangeInterval);

            // ������������� �������
            timers[i] = 0f;
        }
    }

    void Update()
    {
        for (int i = 0; i < torusCount; i++)
        {
            // ������� ��������� ��� ��������
            currentRotationAxes[i] = Vector3.Lerp(currentRotationAxes[i], targetRotationAxes[i], smoothness * Time.deltaTime);
            torusArray[i].transform.Rotate(currentRotationAxes[i] * rotationSpeed * Time.deltaTime);

            // ���������� �������
            timers[i] += Time.deltaTime;

            // ���� ������ �������� ��������, ������ ��� ��������
            if (timers[i] >= changeIntervals[i])
            {
                ChangeRotation(i);
                timers[i] = 0f;
            }
        }
    }

    void ChangeRotation(int index)
    {
        // ��������� ����� ��������� ��� �������� ��� ����������� ����
        targetRotationAxes[index] = Random.onUnitSphere;

        // �����������: ����� �������� �������� ��� ����� ����
        changeIntervals[index] = Random.Range(minChangeInterval, maxChangeInterval);
    }
}