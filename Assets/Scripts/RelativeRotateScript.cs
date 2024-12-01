using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRotateScript : MonoBehaviour
{
    public Transform targetObject;  // ������, ������������ �������� ���������� ��������
    public float rotationSpeed = 10f;  // �������� �������� (������� � �������)

    void Update()
    {
        // ��������� ����������� �� �������� ������� � targetObject
        float angle = targetObject.eulerAngles.x;

        // ������� ����� ������ ��� X � ����������� �� ���� �������� ������
        transform.rotation = Quaternion.Euler(angle * rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}