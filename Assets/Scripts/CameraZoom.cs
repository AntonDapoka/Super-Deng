using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;  // ������ �� �������� ������
    public float zoomFactor = 1.5f;  // ������ ���������� ����
    public float zoomTime = 0.5f;  // ����� ��� ����������� � ������ (� ��������)
    public float returnTime = 0.2f;  // ����� ��� ����������� � ��������� ���� (� ��������)
    public float beatsPerMinute = 90f;  // ��������� BPM (������ � ������)

    private float originalFOV;  // �������� ���� ������ ������
    private float targetFOV;  // ������� ���� ������ ������

    void Start()
    {
        originalFOV = mainCamera.fieldOfView;
        targetFOV = originalFOV / zoomFactor;

        StartCoroutine(ZoomCamera());
    }

    IEnumerator ZoomCamera()
    {
        float zoomSpeed = (targetFOV - mainCamera.fieldOfView) / (zoomTime * (beatsPerMinute / 60f));
        float elapsedTime = 0f;

        // �������� ������
        while (elapsedTime < zoomTime)
        {
            mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ������ � ��������� ����
        float returnSpeed = (originalFOV - mainCamera.fieldOfView) / returnTime;
        elapsedTime = 0f;

        while (elapsedTime < returnTime)
        {
            mainCamera.fieldOfView += returnSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ����������� ���������� ��������
        mainCamera.fieldOfView = originalFOV;
    }
}