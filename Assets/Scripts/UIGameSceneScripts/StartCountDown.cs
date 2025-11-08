using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour // Скрипт отвечает за анимацию стартового отсчета 3..2..1..
{
    [SerializeField] private RhythmManager RM;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private LaunchObstaclesScript LOS;

    public void StartStartCountDown() //Запуск отсчета
    {
        StartCoroutine(CountDownRoutine());
    }

    private IEnumerator CountDownRoutine()
    {
        float delay = RM.beatInterval;           //Временной промежуток между цифрами отсчета равен бпм
        countDownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(delay);
        }
        LOS.StartLaunchObstacles();             //Включаем основной функционал уровня
        countDownText.text = "GO!";
        yield return new WaitForSeconds(delay);
        countDownText.gameObject.SetActive(false);
    }
}

