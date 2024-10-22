using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeToSettingsScript : MonoBehaviour
{
    public Image pauseMenuImage; // Image ��� ����������� ���� �����
    private bool isPaused = false; // ��������� ����, �� ����� �� ����

    private void Start()
    {
        // �������� Image � ������ ����
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // ����������� ������� ������� Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    // ����� ��� ����� ���� � ��������� Image
    public void Pause()
    {
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(true); // �������� �����������
        }
        Time.timeScale = 0f; // ������������� ������� �����
        isPaused = true;
    }

    // ����� ��� ������ ����� � ���������� Image
    public void Resume()
    {
        if (pauseMenuImage != null)
        {
            pauseMenuImage.gameObject.SetActive(false); // ��������� �����������
        }
        Time.timeScale = 1f; // ���������� ���������� �������� ����
        isPaused = false;
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f; isPaused = false;
        SceneManager.LoadScene(sceneIndex);
    }
}
