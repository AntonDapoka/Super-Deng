using UnityEngine;
using UnityEngine.UI;
using TMPro; // ���� �� ����������� TextMeshPro ��� ������

public class TimerController : MonoBehaviour
{
    public Slider timerSlider; // ������ �� ��������
    public TextMeshProUGUI timerText; // ������ �� ��������� ���� (����������� Text ������ TextMeshProUGUI, ���� ����������� ����������� �����)

    private float timeElapsed = 0f; // ��������� �����
    private float totalTime = 120f; // ����� ����� � �������� (2 ������)
    private bool timerIsRunning = false;

    void Start()
    {
        // ������������� ��������
        timerSlider.maxValue = totalTime;
        timerSlider.value = 0;

        // ������ �������
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeElapsed < totalTime)
            {
                timeElapsed += Time.deltaTime;
                UpdateTimerDisplay(timeElapsed);
            }
            else
            {
                timeElapsed = totalTime;
                timerIsRunning = false;
                UpdateTimerDisplay(timeElapsed);
                // ������ ��������
                Debug.Log("������ ��������!");
            }
        }
    }

    void UpdateTimerDisplay(float time)
    {
        // ���������� ��������
        timerSlider.value = time;

        // ���������� ���������� �����������
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}