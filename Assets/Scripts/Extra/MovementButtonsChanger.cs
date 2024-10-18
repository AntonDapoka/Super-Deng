using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Reflection;

public class MovementButtonsChanger : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // ������ ��� ���� ��� ������
    [SerializeField] private TextMeshProUGUI[] buttonTexts; // ������ ��� ������� �� ������� (TMPro)
    [SerializeField] private Image[] buttonImages;

    private int currentButtonIndex = -1; // ������ ������� ������, ��� ������� ��� ������� �������

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttonTexts[i].text = "D"; // ����������� ����� �� ���� �������
            buttons[i].onClick.AddListener(() => OnButtonClick(index)); // ����������� ���������� ��� ������ ������
        }
    }

    private void Update()
    {
        // ���� �� ��� ������� ������� ��� ����� �� ������
        if (currentButtonIndex != -1)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    // ���� ������ �������, ������� �������� ������, ������ ��� ��������
                    if (Input.GetKeyDown(keyCode) && IsValidKey(keyCode))
                    {
                        buttonTexts[currentButtonIndex].text = keyCode.ToString(); // �������� ����� �� ��������� ������
                        buttonImages[currentButtonIndex].enabled = true;
                        currentButtonIndex = -1; // ���������� ������
                        break;
                    }
                }
            }
        }
    }

    void OnButtonClick(int index)
    {
        // ��� ������� �� ������ �������� ����� � �������� ����� ������� ������� ��� ���������� ������
        buttonTexts[index].text = "Press";
        currentButtonIndex = index;
        buttonImages[index].enabled = false;
    }

    // ���������, �������� �� ������� ������, ������ ��� ��������
    bool IsValidKey(KeyCode keyCode)
    {
        return (keyCode >= KeyCode.A && keyCode <= KeyCode.Z) || // �������� �� �����
               (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9) || // �������� �� �����
               (keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9) || // �������� �� ����� �� �������� ����������
               (keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow); // �������� �� �������
    }
}