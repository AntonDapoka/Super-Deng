using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonScript : MonoBehaviour
{
    public Button reloadButton; // ���������� ��� ������ UI

    void Start()
    {
        // ����������� ����� � ������� ������� �� ������
        if (reloadButton != null)
        {
            reloadButton.onClick.AddListener(ReloadCurrentScene);
        }
        else
        {
            Debug.LogError("������ �� ���������!");
        }
    }

    public void ReloadCurrentScene()
    {
        // �������� ������� ����� � ������������� ��
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
