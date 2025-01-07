using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public bool isTurnOn = false;
    public float timeElapsed = 0f;
    public float totalTime = 130f;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private TextMeshProUGUI timerText; 
    [SerializeField] private WinScript WS;

    public void StartTimerController(float totalTrackTime)
    {
        totalTime = totalTrackTime;
        timerSlider.maxValue = totalTime;
        timerSlider.value = 0;
        isTurnOn = true;
    }

    private void FixedUpdate()
    {
        if (isTurnOn)
        {
            if (timeElapsed < totalTime)
            {
                timeElapsed += Time.deltaTime;
                UpdateTimerDisplay(timeElapsed);
            }
            else
            {
                timeElapsed = totalTime;
                isTurnOn = false;
                UpdateTimerDisplay(timeElapsed);
                WS.Win();
            }
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        timerSlider.value = time;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}