using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour
{
    [SerializeField] private RhythmManager RM;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private LaunchObstaclesScript LOS;

    public void StartStartCountDown()
    {
        StartCoroutine(CountDownRoutine());
    }

    private IEnumerator CountDownRoutine()
    {
        float delay = RM.beatInterval;
        countDownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(delay);
        }
        LOS.StartLaunchObstacles();
        countDownText.text = "GO!";
        yield return new WaitForSeconds(delay);
        countDownText.gameObject.SetActive(false);
    }
}

