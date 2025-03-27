using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcosphereRotateToCameraScript : MonoBehaviour
{
    [SerializeField] private Transform transformIcoSphere;
    public Transform target; // ������� ������, �� ������� ������ �������� �������� ������
    public Transform childObject; // �������� ������ �����
    public float rotationSpeed = 5f; // �������� ��������

    void Update()
    {
        if (target == null || childObject == null) return;

        // ����������� �� ��������� ������� � ����
        Vector3 directionToTarget = target.position - childObject.position;
        if (directionToTarget == Vector3.zero) return;

        // �������� ��������, ����� ��� Y ��������� ������� �������� �� ����
        Quaternion targetRotation = Quaternion.FromToRotation(-childObject.up, directionToTarget) * transformIcoSphere.rotation;

        // ������� �������� ���� �����
        transformIcoSphere.rotation = Quaternion.Slerp(transformIcoSphere.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

