using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndestroyableObjectScript : MonoBehaviour
{
    private void Awake()
    {
        // ���������, ���� �� ��� ������� ����� ����
        UndestroyableObjectScript[] objects = FindObjectsOfType<UndestroyableObjectScript>();

        if (objects.Length > 1)
        {
            // ���� ������ ��� ����������, ���������� ����� (��������)
            Destroy(gameObject);
            return;
        }

        // ���������, �� �������� �� ������� ����� 2-� �� �������
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            // ������ ������ ��������������� ��� �������� ����� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���������� ������, ���� ��� ����� � �������� 2
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // ������� ������, ���� ����� ����� ������, �������� �� 0 ��� 1
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Destroy(gameObject);
        }
    }
}
