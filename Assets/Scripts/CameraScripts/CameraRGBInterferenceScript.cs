using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRGBInterferenceScript : MonoBehaviour
{
    [SerializeField] private RGBShiftEffect RGBShiftEffect;

    public float variableValue = 0f; // �������� ����������
    public float targetValue = 0.1f; // ������� ��������, �� �������� ������������� ����������
    public float speed = 0.01f; // �������� ��������� ����������

    private bool isIncreasing = false; // ����������� ��������� ��������
    private bool isChanging = false; // ���������, �������� �� �������� ������
    private float initialValue; // ��������� �������� ����������

    void Start()
    {
        initialValue = variableValue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) // ������� ������� (��������, ������)
        {
            isIncreasing = !isIncreasing; // ����������� �����������
            isChanging = true; // ��������� ��������� ��������
            RGBShiftEffect.on = true;
        }

        if (isChanging)
        {
            if (isIncreasing)
            {
                // ����������� ��������
                variableValue = Mathf.MoveTowards(variableValue, targetValue, speed * Time.deltaTime);
                RGBShiftEffect.amount = variableValue;
                if (Mathf.Approximately(variableValue, targetValue))
                {
                    isChanging = false; // ������������� ���������, ���� �������� ����
                }
            }
            else
            {
                // ��������� ��������
                RGBShiftEffect.amount = variableValue;
                variableValue = Mathf.MoveTowards(variableValue, initialValue, speed * Time.deltaTime);
                if (Mathf.Approximately(variableValue, initialValue))
                {
                    isChanging = false; // ������������� ���������, ���� �������� ���������� ��������
                    RGBShiftEffect.on = false;
                }
            }
        }
    }
}